
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SAPbobsCOM;
using GestionSolicitud.HelperClasses;
using GestionSolicitud.HelperClasses.Interfaces;
using GestionSolicitud.Models;

namespace GestionSolicitud.HelperClasses
{
    public class InventoryService
    {
        private readonly SAPService _sapService;

        private readonly ILogger<InventoryService> _logger;

        private readonly IBackgroundTaskQueue _taskQueue;

        public InventoryService(SAPService sapService, ILogger<InventoryService> logger, IBackgroundTaskQueue taskQueue)
        {
            _sapService = sapService;
            _logger = logger;
            _taskQueue = taskQueue;
        }

        public async Task<string> CreateInventoryDraftBackground(List<VwSolicitudDetalle> detalle, int indec)
        {
            string processId = Guid.NewGuid().ToString();
            await _taskQueue.QueueBackgroundWorkItemAsync(async delegate (CancellationToken token)
            {
                try
                {
                    await Task.Run(delegate
                    {
                        CreateInventoryTransferDraft(detalle, indec);
                    }, token);
                    _logger.LogInformation("Inventory draft created successfully. Process ID: " + processId);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error creating inventory draft. Process ID: " + processId);
                }
            });
            return processId;
        }

        private void CreateInventoryTransferDraft(List<VwSolicitudDetalle> detalle, int indec)
        {
            Company sapCompany = null;
            Documents oGoodsIssueDraft = null;
            int draftEntry = -1;
            try
            {
                sapCompany = _sapService.GetCompany();
               // oGoodsIssueDraft = (Documents)sapCompany.GetBusinessObject(BoObjectTypes.oInventoryTransferRequest);
                oGoodsIssueDraft.DocObjectCode = BoObjectTypes.oInventoryGenExit;
                oGoodsIssueDraft.CardCode = null;
                oGoodsIssueDraft.DocDate = DateTime.Now;
                oGoodsIssueDraft.TaxDate = DateTime.Now;
                oGoodsIssueDraft.DocDueDate = DateTime.Now;
                oGoodsIssueDraft.UserFields.Fields.Item("U_FechRecIVA").Value = DateTime.Now;
                oGoodsIssueDraft.Comments = detalle[0].ComentariosDet;
                oGoodsIssueDraft.Reference2 = indec.ToString();
                for (int i = 0; i < detalle.Count; i++)
                {
                    oGoodsIssueDraft.Lines.SetCurrentLine(i);
                    oGoodsIssueDraft.Lines.ItemCode = detalle[i].IdProducto;
                    oGoodsIssueDraft.Lines.Quantity = (double)detalle[i].Cantidad.Value;
                    oGoodsIssueDraft.Lines.WarehouseCode = detalle[i].Bodega.ToString();
                    oGoodsIssueDraft.Lines.Add();
                }
                if (oGoodsIssueDraft.Add() != 0)
                {
                    string errMsg = sapCompany.GetLastErrorDescription();
                    int errCode = sapCompany.GetLastErrorCode();
                    Console.WriteLine($"Error al crear el borrador de Salida ({errCode}): {errMsg}");
                    draftEntry = -1;
                }
                else
                {
                    string draftKey = sapCompany.GetNewObjectKey();
                    if (int.TryParse(draftKey, out draftEntry))
                    {
                        Console.WriteLine($"Borrador de Salida de Mercancías creado con éxito. DraftEntry: {draftEntry}");
                    }
                    else
                    {
                        Console.WriteLine("Borrador creado, pero no se pudo obtener el DraftEntry (Key: " + draftKey + ").");
                        draftEntry = -2;
                    }
                }
            }
            finally
            {
                if (oGoodsIssueDraft != null)
                {
                    Marshal.ReleaseComObject(oGoodsIssueDraft);
                }
            }
        }
    }
}
