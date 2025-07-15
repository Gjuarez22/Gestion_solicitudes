using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionSolicitud.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using GestionSolicitud.ViewModels;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;
using GestionSolicitud.DTESupplier.Logica;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using GestionSolicitud.HelperClasses;
using GestionSolicitud.Models;

namespace GestionSolicitud.Controllers
{
    [Authorize]
    public class FlsolicitudesController : Controller
    {
        private readonly DbFlujosTestContext _context;
       

        public FlsolicitudesController(DbFlujosTestContext context)
        {
            _context = context;
          //  _clasesSAP = clasesSAP;
        }

        // GET: Flsolicitudes
        public async Task<IActionResult> Index()
        {
            var claimIdUsuario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var claimRol = User.FindFirst(ClaimTypes.Role)?.Value;

            int.TryParse(claimIdUsuario, out int idUsuario);
            var idRol = _context.Flrols
                .Where(r => r.NombreRol == claimRol)
                .Select(r=>r.IdRol)
                .FirstOrDefault();

            var spListadoSolicitudes = _context.spListadoSoliciturdes(idUsuario,idRol);
                
            return View(spListadoSolicitudes);
        }

        // GET: Flsolicitudes
        public async Task<IActionResult> IndexAll()
        {
            var claimIdUsuario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var claimRol = User.FindFirst(ClaimTypes.Role)?.Value;

            int.TryParse(claimIdUsuario, out int idUsuario);
            var idRol = _context.Flrols
                .Where(r => r.NombreRol == claimRol)
                .Select(r => r.IdRol)
                .FirstOrDefault();

            var spListadoSolicitudes = _context.spListadoSoliciturdesAll(idUsuario, idRol);

            return View(spListadoSolicitudes);
        }


        // GET: Flsolicitudes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flsolicitud = await _context.Flsolicituds
                .Include(f => f.IdAreaNavigation)
                .Include(f => f.IdSolicitanteNavigation)
                .Include(f => f.IdStatusNavigation)
                .Include(f => f.IdTipoSolicitudNavigation)
                .Include(f => f.FlsolicitudDets)
                
                .FirstOrDefaultAsync(m => m.IdSolicitud == id);

            if (flsolicitud == null)
            {
                return NotFound();
            }

         
            var productos = await _context.VwProductos.ToListAsync();

            foreach (var detalle in flsolicitud.FlsolicitudDets)
            {
                if (!string.IsNullOrWhiteSpace(detalle.IdProducto))
                {
                    var producto = productos.FirstOrDefault(p => p.Codigo == detalle.IdProducto);
                    detalle.descripcionProducto = producto?.ItemName;
                    detalle.UbicacionProducto = producto?.AreaRepuesto;

                    detalle.descripcionProducto = producto?.ItemName ?? "(Producto no encontrado)";
                    detalle.UbicacionProducto = producto?.AreaRepuesto ?? "(Ubicacion no encontrada)";
                }

            }


            return PartialView(flsolicitud);
        }

        // GET: Flsolicitudes/Create
        public IActionResult Create()
        {
            var areas = _context.Flareas.ToList();          
            var tipoSolicitudes = _context.FltipoSolicituds.ToList();
         //  var estados = _context.Flstatuses.ToList();

            var solicitudVm = new SolicitudViewModel();
            solicitudVm.Areas = new SelectList(areas, "IdArea", "NombreArea");
           // solicitudVm.Estados = new SelectList(estados, "IdStatus", "NombreStatus");
            solicitudVm.TiposSolicitud = new SelectList(tipoSolicitudes, "IdTipoSolicitud", "NombreTipoSolicitud");

          //  var productos = _context.VwProductos.ToList();
            //var producto = _context.VwProductos.ToListAsync();
            //solicitudVm.Productos = new SelectList(producto, "Codigo", "ItemName");
           // solicitudVm.Productos = new SelectList(productos, "Codigo", "ItemName");
            return View(solicitudVm);
        }
        
        // GET: Flsolicitudes/Create
        /* es Wendy?
        public IActionResult CrearDetalle(int idSolicitud)
        {
            var areas = _context.Flareas.ToList();          
            var tipoSolicitudes = _context.FltipoSolicituds.ToList();
         //  var estados = _context.Flstatuses.ToList();

            var solicitudVm = new SolicitudViewModel();
            solicitudVm.Areas = new SelectList(areas, "IdArea", "NombreArea");
           // solicitudVm.Estados = new SelectList(estados, "IdStatus", "NombreStatus");
            solicitudVm.TiposSolicitud = new SelectList(tipoSolicitudes, "IdTipoSolicitud", "NombreTipoSolicitud");
            solicitudVm.Maquinas = new SelectList(maquinas, "IdMaquina", "NombreMaquina");

          //  var productos = _context.VwProductos.ToList();
            //var producto = _context.VwProductos.ToListAsync();
            //solicitudVm.Productos = new SelectList(producto, "Codigo", "ItemName");
           // solicitudVm.Productos = new SelectList(productos, "Codigo", "ItemName");
            return View(solicitudVm);
        }*/
           
        // GET: Flsolicitudes/Create
        public IActionResult CrearDetalle(int idSolicitud)
        {
            var areas = _context.Flareas.ToList();
            var tipoSolicitudes = _context.FltipoSolicituds.ToList();
            var estados = _context.Flstatuses.ToList();
            var maquinas = _context.Flmaquinas.ToList();

            var solicitudVm = new SolicitudViewModel();
            solicitudVm.Areas = new SelectList(areas, "IdArea", "NombreArea");
            solicitudVm.Estados = new SelectList(estados, "IdStatus", "NombreStatus");
            solicitudVm.TiposSolicitud = new SelectList(tipoSolicitudes, "IdTipoSolicitud", "NombreTipoSolicitud");
            solicitudVm.Maquinas = new SelectList(maquinas, "IdMaquina", "NombreMaquina");

            return View(solicitudVm);
        }


        [HttpPost]                                                                                                                                                                                                             
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create( SolicitudViewModel flsolicitud)
        {
            if (ModelState.IsValid)
            {
                
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); //Obtener el id de la sesión actual
                
                var solicitud = new Flsolicitud();
                solicitud.Fecha =  DateTime.Now;
                solicitud.IdSolicitante = userId;
                solicitud.IdTipoSolicitud = flsolicitud.IdTipoSolicitud;
                solicitud.IdStatus = "PRE";
                solicitud.DocNumErp = null;
                solicitud.Comentarios = flsolicitud.Comentarios;
                solicitud.IdArea = flsolicitud.IdArea;
                solicitud.Cancelada = false;
                
                _context.Add(solicitud);
                await _context.SaveChangesAsync();
                int idnewSolicitud = solicitud.IdSolicitud;

                ///requiere de un valor para el redireccionamiento..
                return RedirectToAction(nameof(CreateDetalles), new { id = idnewSolicitud });

            }
            return View(flsolicitud);
        }
                  
        // GET: Flsolicitudes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flsolicitud = await _context.Flsolicituds.FindAsync(id);
            if (flsolicitud == null)
            {
                return NotFound();
            }
            ViewData["IdArea"] = new SelectList(_context.Flareas, "IdArea", "IdArea", flsolicitud.IdArea);
            ViewData["IdSolicitante"] = new SelectList(_context.Flusuarios, "IdUsuario", "IdUsuario", flsolicitud.IdSolicitante);
            ViewData["IdStatus"] = new SelectList(_context.Flstatuses, "IdStatus", "IdStatus", flsolicitud.IdStatus);
            ViewData["IdTipoSolicitud"] = new SelectList(_context.FltipoSolicituds, "IdTipoSolicitud", "IdTipoSolicitud", flsolicitud.IdTipoSolicitud);
            return View(flsolicitud);
        }

        // POST: Flsolicitudes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSolicitud,Fecha,IdSolicitante,IdTipoSolicitud,IdArea,IdStatus,DocNumErp,Comentarios,Cancelada,Reenviada")] Flsolicitud flsolicitud)
        {
            if (id != flsolicitud.IdSolicitud)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flsolicitud);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlsolicitudExists(flsolicitud.IdSolicitud))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdArea"] = new SelectList(_context.Flareas, "IdArea", "IdArea", flsolicitud.IdArea);
            ViewData["IdSolicitante"] = new SelectList(_context.Flusuarios, "IdUsuario", "IdUsuario", flsolicitud.IdSolicitante);
            ViewData["IdStatus"] = new SelectList(_context.Flstatuses, "IdStatus", "IdStatus", flsolicitud.IdStatus);
            ViewData["IdTipoSolicitud"] = new SelectList(_context.FltipoSolicituds, "IdTipoSolicitud", "IdTipoSolicitud", flsolicitud.IdTipoSolicitud);
            return View(flsolicitud);
        }

        // GET: Flsolicitudes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flsolicitud = await _context.Flsolicituds
                .Include(f => f.IdAreaNavigation)
                .Include(f => f.IdSolicitanteNavigation)
                .Include(f => f.IdStatusNavigation)
                .Include(f => f.IdTipoSolicitudNavigation)
                .FirstOrDefaultAsync(m => m.IdSolicitud == id);

            if (flsolicitud == null)
            {
                return NotFound();
            }

            return View(flsolicitud);
        }

        // POST: Flsolicitudes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flsolicitud = await _context.Flsolicituds.FindAsync(id);
            if (flsolicitud != null)
            {
                _context.Flsolicituds.Remove(flsolicitud);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlsolicitudExists(int id)
        {
            return _context.Flsolicituds.Any(e => e.IdSolicitud == id);
        }


        [Authorize(Roles = "Solicitante")]
        [HttpPost]
        public async Task<JsonResult> Recibio([FromBody] int id)
        {
            //id de la solicitud
            await _context.Database.ExecuteSqlRawAsync("EXEC sp_RecibidoSolicitante @p0", id); //Ejecuta el sp de recibioSolcitiante
            //Ya que no devuvle ninguna tala, no se realizo un modelo para la tabla, de lo contrario hay que hacerlo

            return Json(new { success = true, message = "Estado de solicitud: "+id+" actualizado correctamente." });
        }

       
        [Authorize(Roles = "EncargadoBodega,Administrador")]
        [HttpPost]
        public async Task<JsonResult> Entregar([FromBody] DatosAutorizacion datos)
        {

            //id de la solicitud
            var solicitud = await _context.Flsolicituds
                .FindAsync(datos.Id); //Buscamos la solicuitud para obtener el di del estado
            int idenc = datos.Id; 
            using var conn = _context.Database.GetDbConnection();
            using var command = conn.CreateCommand();

            command.CommandText = "sp_EntregaBodega_ent";
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
            AddParam("@IdSolicitud",datos.Id.ToString());
            AddParam("@Estatus", solicitud.IdStatus);
            AddParam("@Usuario", datos.IdUsuario);
         
            AddParam("@Mensaje", "", DbType.String, ParameterDirection.Output, 250);
            AddParam("@CodigoResultado", 0, DbType.Int32, ParameterDirection.Output);

            await conn.OpenAsync();
            await command.ExecuteNonQueryAsync();

            string mensaje = command.Parameters["@Mensaje"].Value?.ToString();
            int codigoResultado = Convert.ToInt32(command.Parameters["@CodigoResultado"].Value);

            int id_sol  = datos.Id;

            List<VwSolicitudDetalle> result = _context.VwSolicitudDetalles.Where((VwSolicitudDetalle o) => o.IdSolicitud == id_sol).ToList();
            if (result.Count > 0)
            {
              //  await _clasesSAP.CreateGoodsIssueDraft(result, idenc);
            }

            return Json(new
            {
                success = true,
                message = mensaje,
                codigoResultado
            });

            //id de la solicitud
            //await _context.Database.ExecuteSqlRawAsync("EXEC sp_EntregaEncargadoBodega @IdSolicitud", id); //Ejecuta el sp de recibioSolcitiante

            //return Json(new { success = true, message = "Estado de solicitud: " + id + " actualizado correctamente." });
        }

        public class DatosAutorizacion
        {
            public int Id { get; set; }
            public int IdUsuario { get; set; }
        }


        [Authorize(Roles = "Autorizador")]
        [HttpPost]
        public async Task<JsonResult> Autorizar([FromBody] DatosAutorizacion datos)
        {
            //id de la solicitud
            var solicitud = await _context.Flsolicituds
                .FindAsync(datos.Id); //Buscamos la solicuitud para obtener el di del estado

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
            AddParam("@IdSolicitud", datos.Id.ToString());
            AddParam("@Estatus", solicitud.IdStatus);
            AddParam("@Usuario", datos.IdUsuario);
            AddParam("@Plataforma", "WEB");
            AddParam("@Email", "NO");
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

            /*int filasAfectadas = await _context.Database.ExecuteSqlRawAsync("EXEC spAutorizacion_aut @IdSolicitud = {0}, @Estatus = {1}, @Usuario = {2}, @Plataforma = {3} ",
                  datos.Id, solicitud.IdStatus, datos.IdUsuario, "WEB"); //Ejecuta el sp de spAutorizacion
            
               
            return Json(new { success = true, message = "Solicitud: "+ datos.Id +" autorizada correctamente." });*/

        }

        public class DatosAutorizacion_rec
        {
            public int IdUsuario { get; set; }
            public int Id { get; set; }
            

        }

        [Authorize(Roles = "Autorizador")]
        [HttpPost]
        public async Task<JsonResult> Rechazo([FromBody] DatosAutorizacion_rec datos)
        {
            //id de la solicitud
            var solicitud = await _context.Flsolicituds
                .FindAsync(datos.Id); //Buscamos la solicuitud para obtener el di del estado

            using var conn = _context.Database.GetDbConnection();
            using var command = conn.CreateCommand();

            command.CommandText = "spAutorizacion_rec";
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
            AddParam("@IdSolicitud", datos.Id.ToString());
            AddParam("@Estatus", solicitud.IdStatus);
            AddParam("@Usuario", datos.IdUsuario);
            AddParam("@Plataforma", "WEB");
            AddParam("@Email", "NO");
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

            /*int filasAfectadas = await _context.Database.ExecuteSqlRawAsync("EXEC spAutorizacion_aut @IdSolicitud = {0}, @Estatus = {1}, @Usuario = {2}, @Plataforma = {3} ",
                  datos.Id, solicitud.IdStatus, datos.IdUsuario, "WEB"); //Ejecuta el sp de spAutorizacion
            
               
            return Json(new { success = true, message = "Solicitud: "+ datos.Id +" autorizada correctamente." });*/

        }

        [HttpPost]
        public async Task<JsonResult> cancelar([FromBody] DatosAutorizacion_rec datos)
        {
            //id de la solicitud
            var solicitud = await _context.Flsolicituds
                .FindAsync(datos.Id); //Buscamos la solicuitud para obtener el di del estado

            using var conn = _context.Database.GetDbConnection();
            using var command = conn.CreateCommand();

            command.CommandText = "spSolicitud_cancel";
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
            AddParam("@IdSolicitud", datos.Id.ToString());
            AddParam("@Estatus", solicitud.IdStatus);
            AddParam("@Usuario", datos.IdUsuario);
            AddParam("@Comentario", "--");
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

            /*int filasAfectadas = await _context.Database.ExecuteSqlRawAsync("EXEC spAutorizacion_aut @IdSolicitud = {0}, @Estatus = {1}, @Usuario = {2}, @Plataforma = {3} ",
                  datos.Id, solicitud.IdStatus, datos.IdUsuario, "WEB"); //Ejecuta el sp de spAutorizacion

            return Json(new { success = true, message = "Solicitud: "+ datos.Id +" autorizada correctamente." });*/

        }

        [HttpGet("busquedaItem")]
        public IActionResult busquedaItem(string term, int page = 1, int pageSize = 10)
        {
            try
            {
                // Simula una búsqueda en base de datos
                var usuarios = _context.Flusuarios
                    .Where(u => string.IsNullOrEmpty(term) ||
                               u.Nombre.Contains(term))
                    .Select(u => new {
                        id = u.IdUsuario,
                        text = u.Nombre
                    })
                    .ToList();

                var totalCount = _context.Flusuarios
                    .Count(u => string.IsNullOrEmpty(term) ||
                               u.Nombre.Contains(term));

                // Formato esperado por Select2
                var result = new
                {
                    results = usuarios,
                    pagination = new
                    {
                        more = (page * pageSize) < totalCount
                    }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        public IActionResult CreateDetalles(int id)
        {
            return View();
        }

    }
}
