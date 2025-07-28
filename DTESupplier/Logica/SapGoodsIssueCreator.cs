using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SAPbobsCOM;

using GestionSolicitud.HelperClasses;
using GestionSolicitud.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace GestionSolicitud.DTESupplier.Logica
{
    public class SapGoodsIssueCreator 
    {
        private readonly MiServicio _miServicio;

        private Company? _sapCompany;
        private readonly DbFlujosTestContext _context;

        public SapGoodsIssueCreator(MiServicio miServicio, DbFlujosTestContext contex)
        {
            _miServicio = miServicio;
            _context = contex;
        
        } 
        public bool Connect()
        {
            try
            {
                //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_miServicio.GetConnectionSting("SAP"));
                SqlConnectionStringBuilder builder = new(_miServicio.GetConnectionSting("SAP"));

                // Verificar que la cadena de conexión no esté vacía
                string connectionString = _miServicio.GetConnectionSting("SAP");
                Console.WriteLine($"Cadena de conexión: '{connectionString}'");
                Console.WriteLine($"Longitud: {connectionString?.Length ?? 0}");
                Console.WriteLine($"Es null: {connectionString == null}");
                Console.WriteLine($"Es vacía: {string.IsNullOrEmpty(connectionString)}");
                if (string.IsNullOrEmpty(connectionString))
                {
                    Console.WriteLine("Error: La cadena de conexión SAP está vacía");
                    // _context.Database.ExecuteSqlRawAsync("EXEC sp_EntregaLog @IdSolicitud = {0}, @Mensaje = {1}", 0, "01Error: La cadena de conexión SAP está vacía");
                    return false;
                }
                else
                {
                     //_context.Database.ExecuteSqlRawAsync("EXEC sp_EntregaLog @IdSolicitud = {0}, @Mensaje = {1}", 0, "01Procesando");
                }


                _sapCompany = new CompanyClass();
                _sapCompany.language = BoSuppLangs.ln_Spanish_La;
                _sapCompany.DbServerType = BoDataServerTypes.dst_MSSQL2014;
                _sapCompany.Server = builder.DataSource;
                _sapCompany.CompanyDB = builder.InitialCatalog;
                _sapCompany.DbServerType = BoDataServerTypes.dst_MSSQL2014;
                _sapCompany.DbUserName = builder.UserID;
                _sapCompany.DbPassword = builder.Password;
                _sapCompany.UserName = _miServicio.GetConnectionSting("UsuarioSAP");
                _sapCompany.Password = _miServicio.GetConnectionSting("PassWordSAP");

                Console.WriteLine($"DataSource: '{builder.DataSource}'");
                Console.WriteLine($"InitialCatalog: '{builder.InitialCatalog}'");
                Console.WriteLine($"UserID: '{builder.UserID}'");
                Console.WriteLine($"Password: '{builder.Password}'");
                Console.WriteLine($"IntegratedSecurity: {builder.IntegratedSecurity}");

                if (_sapCompany.Connect() != 0)
                {
                    _sapCompany.GetLastError(out var errorCode, out var errorMessage);
                    Console.WriteLine($"Error al conectar a SAP B1: ({errorCode}) {errorMessage}");
                   // _context.Database.ExecuteSqlRawAsync("EXEC sp_EntregaLog @IdSolicitud = {0}, @Mensaje = {1}", 0, "02Error al conectar a SAP B1: ");
                    _sapCompany = null;
                    return false;
                }

                Console.WriteLine("Conectado exitosamente a la compañía: " + _sapCompany.CompanyName);
                //_context.Database.ExecuteSqlRawAsync("EXEC sp_EntregaLog @IdSolicitud = {0}, @Mensaje = {1}", 0, "03Conectado exitosamente a la compañía:  " );
                return true;
            }
            catch (Exception ex)
            {
               //_context.Database.ExecuteSqlRawAsync("EXEC sp_EntregaLog @IdSolicitud = {0}, @Mensaje = {1}", 0, "03Excepción durante la conexión a SAP B1 ");
                Console.WriteLine("Excepción durante la conexión a SAP B1: " + ex.Message);
                
                if (_sapCompany != null && !_sapCompany.Connected)
                {
                    Marshal.ReleaseComObject(_sapCompany);
                    _sapCompany = null;
                }
                return false;
            }
        }

        public async Task CreateGoodsIssueDraft(List<VwSolicitudDetalle> detalle, int indec)
        {
            Connect();
            _context.Database.ExecuteSqlRawAsync("EXEC sp_EntregaLog @IdSolicitud = {0}, @Mensaje = {1}", 0, "00 POST SAP B1 ");
            if (_sapCompany == null || !_sapCompany.Connected)
            {
                Console.WriteLine("Error: No hay conexión activa a SAP B1.");
            //  / / await _context.Database.ExecuteSqlRawAsync("EXEC sp_EntregaLog @IdSolicitud = {0}, @Mensaje = {1}", 0, "04Error: No hay conexión activa a SAP B1" );
            }
            else
            {
                //await _context.Database.ExecuteSqlRawAsync("EXEC sp_EntregaLog @IdSolicitud = {0}, @Mensaje = {1}", 0, "04Ningun incoveniente SAP B1");
                Console.WriteLine("Ningun incoveniente SAP B1");
               
            }

            Documents oGoodsIssueDraft = null;
            int draftEntry = -1;
            try
            {
                oGoodsIssueDraft = (Documents)_sapCompany.GetBusinessObject(BoObjectTypes.oDrafts);
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
                    oGoodsIssueDraft.Lines.AccountCode = detalle[i].AcctCode;
                    oGoodsIssueDraft.Lines.Add();
                }
                if (oGoodsIssueDraft.Add() != 0)
                {
                    string errMsg = _sapCompany.GetLastErrorDescription();
                    int errCode = _sapCompany.GetLastErrorCode();
                    Console.WriteLine($"Error al crear el borrador de Salida ({errCode}): {errMsg}");
                  //  _context.Database.ExecuteSqlRawAsync("EXEC sp_EntregaLog @IdSolicitud = {0}, @Mensaje = {1}", 0, "05Error al conectar a SAP B1: " );

                }
                else
                {
                    string draftKey = _sapCompany.GetNewObjectKey();
                    if (int.TryParse(draftKey, out draftEntry))
                    {
                        Console.WriteLine($"Borrador de Salida de Mercancías creado con éxito. DraftEntry: {draftEntry}");
                      //  _context.Database.ExecuteSqlRawAsync("EXEC sp_EntregaLog @IdSolicitud = {0}, @Mensaje = {1}", 0, "06Borrador de Salida de Mercancías creado con éxito. DraftEntry: " + draftKey);

                    }
                    else
                    {
                        Console.WriteLine("Borrador creado, pero no se pudo obtener el DraftEntry (Key: " + draftKey + ").");
                      //  _context.Database.ExecuteSqlRawAsync("EXEC sp_EntregaLog @IdSolicitud = {0}, @Mensaje = {1}", 0, "06Borrador creado, pero no se pudo obtener el DraftEntry (Key: " + draftKey);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Excepción al crear el borrador de Salida: " + ex.Message);
               // _context.Database.ExecuteSqlRawAsync("EXEC sp_EntregaLog @IdSolicitud = {0}, @Mensaje = {1}", 0, "07Borrador creado, pero no se pudo obtener el DraftEntry (Key: " );
            }
            finally
            {
                ReleaseComObject(oGoodsIssueDraft);
            }
        }

        private static void ReleaseComObject(object? obj)
        {
            if (obj == null)
            {
                return;
            }
            try
            {
                Marshal.ReleaseComObject(obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al liberar objeto COM: " + ex.Message);
                //_context.Database.ExecuteSqlRawAsync("EXEC sp_EntregaLog @IdSolicitud = {0}, @Mensaje = {1}", 0, "06Borrador creado, pero no se pudo obtener el DraftEntry (Key: " + ex.Message);
            }
            finally
            {
                obj = null;
            }
        }
    }
}
