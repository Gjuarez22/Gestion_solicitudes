using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
//using SAPbobsCOM;

using GestionSolicitud.HelperClasses;
using GestionSolicitud.Models;
namespace GestionSolicitud.DTESupplier.Logica
{
    public class SapGoodsIssueCreator
    {
        private readonly MiServicio _miServicio;

       // private Company? _sapCompany;

        public SapGoodsIssueCreator(MiServicio miServicio)
        {
            _miServicio = miServicio;
        }

        //public bool Connect()
        //{
        //    try
        //    {
        //        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_miServicio.GetConnectionSting("SAP"));
        //        _sapCompany = new CompanyClass();
        //        _sapCompany.language = BoSuppLangs.ln_Spanish_La;
        //        _sapCompany.DbServerType = BoDataServerTypes.dst_MSSQL2005;
        //        _sapCompany.Server = builder.DataSource;
        //        _sapCompany.CompanyDB = builder.InitialCatalog;
        //        _sapCompany.DbServerType = BoDataServerTypes.dst_MSSQL2005;
        //        _sapCompany.DbUserName = builder.UserID;
        //        _sapCompany.DbPassword = builder.Password;
        //        _sapCompany.UserName = _miServicio.GetConnectionSting("UsuarioSAP");
        //        _sapCompany.Password = _miServicio.GetConnectionSting("PassWordSAP");
        //        if (_sapCompany.Connect() != 0)
        //        {
        //            _sapCompany.GetLastError(out var errorCode, out var errorMessage);
        //            Console.WriteLine($"Error al conectar a SAP B1: ({errorCode}) {errorMessage}");
        //            _sapCompany = null;
        //            return false;
        //        }
        //        Console.WriteLine("Conectado exitosamente a la compañía: " + _sapCompany.CompanyName);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Excepción durante la conexión a SAP B1: " + ex.Message);
        //        if (_sapCompany != null && !_sapCompany.Connected)
        //        {
        //            Marshal.ReleaseComObject(_sapCompany);
        //            _sapCompany = null;
        //        }
        //        return false;
        //    }
        //}

        //public async Task CreateGoodsIssueDraft(List<VwSolicitudDetalle> detalle, int indec)
        //{
        //    Connect();
        //    if (_sapCompany == null || !_sapCompany.Connected)
        //    {
        //        Console.WriteLine("Error: No hay conexión activa a SAP B1.");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Ningun incoveniente SAP B1");
        //    }

        //    Documents oGoodsIssueDraft = null;
        //    int draftEntry = -1;
        //    try
        //    {
        //        oGoodsIssueDraft = (Documents)_sapCompany.GetBusinessObject(BoObjectTypes.oDrafts);
        //        oGoodsIssueDraft.DocObjectCode = BoObjectTypes.oInventoryGenExit;
        //        oGoodsIssueDraft.CardCode = null;
        //        oGoodsIssueDraft.DocDate = DateTime.Now;
        //        oGoodsIssueDraft.TaxDate = DateTime.Now;
        //        oGoodsIssueDraft.DocDueDate = DateTime.Now;
        //        oGoodsIssueDraft.UserFields.Fields.Item("U_FechRecIVA").Value = DateTime.Now;
        //        oGoodsIssueDraft.Comments = detalle[0].ComentariosDet;
        //        oGoodsIssueDraft.Reference2 = indec.ToString();
        //        for (int i = 0; i < detalle.Count; i++)
        //        {
        //            oGoodsIssueDraft.Lines.SetCurrentLine(i);
        //            oGoodsIssueDraft.Lines.ItemCode = detalle[i].IdProducto;
        //            oGoodsIssueDraft.Lines.Quantity = (double)detalle[i].Cantidad.Value;
        //            oGoodsIssueDraft.Lines.WarehouseCode = detalle[i].Bodega.ToString();
        //            oGoodsIssueDraft.Lines.AccountCode = detalle[i].AcctCode;
        //            oGoodsIssueDraft.Lines.Add();
        //        }
        //        if (oGoodsIssueDraft.Add() != 0)
        //        {
        //            string errMsg = _sapCompany.GetLastErrorDescription();
        //            int errCode = _sapCompany.GetLastErrorCode();
        //            Console.WriteLine($"Error al crear el borrador de Salida ({errCode}): {errMsg}");
        //        }
        //        else
        //        {
        //            string draftKey = _sapCompany.GetNewObjectKey();
        //            if (int.TryParse(draftKey, out draftEntry))
        //            {
        //                Console.WriteLine($"Borrador de Salida de Mercancías creado con éxito. DraftEntry: {draftEntry}");
        //            }
        //            else
        //            {
        //                Console.WriteLine("Borrador creado, pero no se pudo obtener el DraftEntry (Key: " + draftKey + ").");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Excepción al crear el borrador de Salida: " + ex.Message);
        //    }
        //    finally
        //    {
        //        ReleaseComObject(oGoodsIssueDraft);
        //    }
        //}

        //private static void ReleaseComObject(object? obj)
        //{
        //    if (obj == null)
        //    {
        //        return;
        //    }
        //    try
        //    {
        //        Marshal.ReleaseComObject(obj);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error al liberar objeto COM: " + ex.Message);
        //    }
        //    finally
        //    {
        //        obj = null;
        //    }
        //}
    }
}
