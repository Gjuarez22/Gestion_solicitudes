using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionSolicitud.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace GestionSolicitud.Controllers

{
    [AllowAnonymous]
    public class AutorizacionController : Controller
    {
        //private readonly DbFlujosContext _context;
        private readonly DbFlujosTestContext _context;

        public AutorizacionController(DbFlujosTestContext dbFlujosContext)
        {
            _context = dbFlujosContext;
        }

        //  [Authorize(Roles = "Administrador")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> SetAuthorization(string IdSolicitud, string Estatus, string asunto , string email)
        {
            try
            {
                int Usuario = 0;
                //string nombreSP = "EXEC spAutorizacion @IdSolicitud,@Estatus";
                //SqlParameter sqlParam = new SqlParameter("@IdSolicitud", IdSolicitud);
                //SqlParameter sqlParam2 = new SqlParameter("@Estatus", Estatus);
                //await _context.Database.ExecuteSqlRawAsync(nombreSP, sqlParam, sqlParam2);
                //if (!(Estatus == "AUT"))
                //{
                //    base.ViewBag.Mensaje = "Solicitud rechazada";
                //}
                //else
                //{
                //    base.ViewBag.Mensaje = "Solicitud aprobada";
                //}
                //return View();

                //id de la solicitud

                var solicitud = await _context.Flsolicituds
                    .FindAsync(Convert.ToInt32(IdSolicitud)); //Buscamos la solicuitud para obtener el di del estado


                using var conn = _context.Database.GetDbConnection();
                using var command = conn.CreateCommand();

                command.CommandText = "spAutorizacion_aut";
                command.CommandType = CommandType.StoredProcedure;

                // Función local para crear parámetros
                void AddParam(string name, object value, DbType? dbType = null, ParameterDirection direction = ParameterDirection.Input, int size = 0)
                {
                    var p = command.CreateParameter();
                    p.ParameterName = name;
                    p.Value = value ?? DBNull.Value;
                    p.Direction = direction;
                    if (dbType.HasValue) p.DbType = dbType.Value;
                    if (size > 0) p.Size = size;
                    command.Parameters.Add(p);
                }

                // Parámetros
                AddParam("@IdSolicitud", IdSolicitud);
                AddParam("@Estatus", Estatus);
                AddParam("@Usuario", Usuario);
                AddParam("@Plataforma", "COR");
                AddParam("@Email", email);
                AddParam("@Mensaje", "", DbType.String, ParameterDirection.Output, 250);
                AddParam("@CodigoResultado", 0, DbType.Int32, ParameterDirection.Output);

                await conn.OpenAsync();
                await command.ExecuteNonQueryAsync();

                string mensaje = command.Parameters["@Mensaje"].Value?.ToString();
                int codigoResultado = Convert.ToInt32(command.Parameters["@CodigoResultado"].Value);

                return Json(new
                {
                    success = true,
                    message = mensaje,
                    codigoResultado
                });
            }
            catch (Exception ex)
            {
                return Content($"Error inesperado: {ex.Message}");
            }

        }
    }
}
