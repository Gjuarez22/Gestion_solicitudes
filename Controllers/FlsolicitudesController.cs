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
using Microsoft.Data.SqlClient;
using System.Net.NetworkInformation;

namespace GestionSolicitud.Controllers
{
    [Authorize]
    public class FlsolicitudesController : Controller
    {
        private readonly DbFlujosTestContext _context;

        public FlsolicitudesController(DbFlujosTestContext context)
        {
            _context = context;
        }

        // GET: Flsolicitudes
        public async Task<IActionResult> Index()
        {

            if (TempData["guardado"] != null)
            {
                ViewBag.guardado = TempData["guardado"];
                // Opcional: TempData se autodestruye después de leerlo
                // Si quieres conservarlo para más usos:
                // TempData.Keep("guardado");
            }

            var claimIdUsuario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var claimRol = User.FindFirst(ClaimTypes.Role)?.Value;

            int.TryParse(claimIdUsuario, out int idUsuario);
            var idRol = _context.Flrols
                .Where(r => r.NombreRol == claimRol)
                .Select(r => r.IdRol)
                .FirstOrDefault();

            var spListadoSolicitudes = _context.spListadoSoliciturdes(idUsuario, idRol);

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

            return PartialView(flsolicitud);
        }

        // GET: Flsolicitudes/Create
        public IActionResult Create()
        {
            var areas = _context.Flareas.ToList();
            var tipoSolicitud = _context.FltipoSolicituds
                .ToList();

            var estados = _context.Flstatuses.ToList();
            var maquinas = _context.Flmaquinas.ToList();


            var tipoSolicitudes = new List<TipoSolicitudViewModel>();
            foreach (var solicitud in tipoSolicitud)
            {
                var flujo = _context.Flflujos.Find(solicitud.IdFlujo);
                var mostrarMaquina = flujo.SeeMaquina == 1 ? true : false;

                tipoSolicitudes.Add(new TipoSolicitudViewModel(solicitud, mostrarMaquina));
            }

            var solicitudVm = new SolicitudViewModel();
            solicitudVm.Areas = new SelectList(areas, "IdArea", "NombreArea");
            solicitudVm.Estados = new SelectList(estados, "IdStatus", "NombreStatus");
            solicitudVm.TiposSolicitud = tipoSolicitudes;
            solicitudVm.Maquinas = new SelectList(maquinas, "IdMaquina", "NombreMaquina");

            return View(solicitudVm);
        }

        // GET: Flsolicitudes/Create
        public IActionResult CrearDetalle(int idSolicitud)
        {
            var areas = _context.Flareas.ToList();
            var tipoSolicitud = _context.FltipoSolicituds
                .ToList();
            var estados = _context.Flstatuses.ToList();
            var maquinas = _context.Flmaquinas.ToList();

            var tipoSolicitudes = new List<TipoSolicitudViewModel>();
            foreach (var solicitud in tipoSolicitud)
            {
                var flujo = _context.Flflujos.Find(solicitud.IdFlujo);
                var mostrarMaquina = flujo.SeeMaquina == 1 ? true : false;

                tipoSolicitudes.Add(new TipoSolicitudViewModel(solicitud, mostrarMaquina));
            }

            var solicitudVm = new SolicitudViewModel();
            solicitudVm.Areas = new SelectList(areas, "IdArea", "NombreArea");
            solicitudVm.Estados = new SelectList(estados, "IdStatus", "NombreStatus");
            solicitudVm.TiposSolicitud = tipoSolicitudes;
            solicitudVm.Maquinas = new SelectList(maquinas, "IdMaquina", "NombreMaquina");

            return View(solicitudVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SolicitudViewModel flsolicitud)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); //Obtener el id de la sesión actual
                /*
                
                var solicitud = new Flsolicitud();
                solicitud.Fecha =  DateTime.Now;
                solicitud.IdSolicitante = userId;
                solicitud.IdTipoSolicitud = flsolicitud.IdTipoSolicitud;
               // solicitud.IdStatus = flsolicitud.IdStatus;
                //solicitud.DocNumErp = flsolicitud.DocNumErp;
                solicitud.Comentarios = flsolicitud.Comentarios;
                solicitud.IdArea = flsolicitud.IdArea;
                solicitud.Cancelada = false;
                
                _context.Add(solicitud);
                await _context.SaveChangesAsync();*/
                // Parámetro de salida
                var nuevoIdParam = new SqlParameter
                {
                    ParameterName = "@NuevoID",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                // Ejecutar el SP
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC [dbo].[spAgregarSolicitudEnc] @IdSolicitante, @IdTipoSolicitud, @IdArea, @IdStatus, @Comentarios, @NuevoID OUTPUT",
                    new SqlParameter("@IdSolicitante", userId),
                    new SqlParameter("@IdTipoSolicitud", flsolicitud.IdTipoSolicitud),
                    new SqlParameter("@IdArea", flsolicitud.IdArea),
                    new SqlParameter("@IdStatus", "PRE"),
                    new SqlParameter("@Comentarios", flsolicitud.Comentarios),
                    nuevoIdParam
                );
                int nuevoId = (int)nuevoIdParam.Value;

                foreach (var item in flsolicitud.detalle)
                {
                    var idSolicitud = nuevoId;
                    var idProducto = item.idProducto;
                    var cantidad = item.cantidad;
                    var idMaquina = item.idMaquina;

                    var filasAfectadas = await _context.Database.ExecuteSqlRawAsync(
                        "EXEC [dbo].[spAgregarSolicitudDet] @IdSolicitud, @IdProducto, @cantidad, @IdMaquina",
                        new SqlParameter("@IdSolicitud", idSolicitud),
                        new SqlParameter("@IdProducto", idProducto),
                        new SqlParameter("@cantidad", cantidad),
                        new SqlParameter("@IdMaquina", idMaquina)
                    );
                }

                // En tu Action origen
                TempData["guardado"] = "Registro ingresado correctamente";
                return RedirectToAction(nameof(Index));
            }

            var areas = _context.Flareas.ToList();
            var tipoSolicitud = _context.FltipoSolicituds
                .ToList();
            var estados = _context.Flstatuses.ToList();
            var maquinas = _context.Flmaquinas.ToList();

            var tipoSolicitudes = new List<TipoSolicitudViewModel>();
            foreach (var solicitud in tipoSolicitud)
            {
                var flujo = _context.Flflujos.Find(solicitud.IdFlujo);
                var mostrarMaquina = flujo.SeeMaquina == 1 ? true : false;

                tipoSolicitudes.Add(new TipoSolicitudViewModel(solicitud, mostrarMaquina));
            }

            var solicitudVm = new SolicitudViewModel();
            solicitudVm.Areas = new SelectList(areas, "IdArea", "NombreArea");
            solicitudVm.Estados = new SelectList(estados, "IdStatus", "NombreStatus");
            solicitudVm.TiposSolicitud = tipoSolicitudes;
            solicitudVm.Maquinas = new SelectList(maquinas, "IdMaquina", "NombreMaquina");
            return View(solicitudVm);
        }

        // GET: Flsolicitudes/Edit/5
        public async Task<IActionResult> Editar(int? id)
        {
            var solicitudEncontrada = await _context.Flsolicituds.FindAsync(id);
            var detalle = await _context.FlsolicitudDets.Where(x=>x.IdSolicitud == id).ToListAsync();

            var areas = _context.Flareas.ToList();
            var tipoSolicitud = _context.FltipoSolicituds
                .ToList();
            var estados = _context.Flstatuses.ToList();
            var maquinas = _context.Flmaquinas.ToList();

            var tipoSolicitudes = new List<TipoSolicitudViewModel>();
            foreach (var solicitud in tipoSolicitud)
            {
                var flujo = _context.Flflujos.Find(solicitud.IdFlujo);
                var mostrarMaquina = flujo.SeeMaquina == 1 ? true : false;

                tipoSolicitudes.Add(new TipoSolicitudViewModel(solicitud, mostrarMaquina));
            }

            var solicitudVm = new SolicitudViewModel();
            solicitudVm.Areas = new SelectList(areas, "IdArea", "NombreArea");
            solicitudVm.Estados = new SelectList(estados, "IdStatus", "NombreStatus");
            solicitudVm.TiposSolicitud = tipoSolicitudes;
            solicitudVm.Maquinas = new SelectList(maquinas, "IdMaquina", "NombreMaquina");

            solicitudVm.IdTipoSolicitud = solicitudEncontrada.IdTipoSolicitud??0;
            solicitudVm.IdArea = solicitudEncontrada.IdArea??0;
            solicitudVm.Comentarios = solicitudEncontrada.Comentarios;
            solicitudVm.detalle = new List<DetalleSolicitudLinea>();

            
            foreach (var det in detalle)
            {
                var prod= _context.VwProductos2s.FirstOrDefault(x => x.Id == int.Parse(det.IdProducto));

                if (prod != null)
                {
                    solicitudVm.detalle.Add(new DetalleSolicitudLinea()
                    {
                        cantidad = (int)det.Cantidad,
                        maquinaString = det.IdMaquina != null ? maquinas.Where(x => x.IdMaquina == det.IdMaquina).FirstOrDefault().NombreMaquina : "N/A",
                        codigo = prod.Codigo,
                        descripcion = prod.ItemName,
                        idProducto = int.Parse(det.IdProducto),
                        idMaquina = det.IdMaquina
                    });
                }
            }
            solicitudVm.IdSolicitud = solicitudEncontrada.IdSolicitud;
            return View(solicitudVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(SolicitudViewModel flsolicitud)
        {
            if (ModelState.IsValid)
            {
                var solicitudEncontrada = _context.Flsolicituds.FirstOrDefault(x => x.IdSolicitud == flsolicitud.IdSolicitud);
                var detalleEncontrado = _context.FlsolicitudDets.Where(x => x.IdSolicitud == flsolicitud.IdSolicitud).ToList();
                
                    //eliminamos el detalle anterior
                    _context.FlsolicitudDets.RemoveRange(detalleEncontrado);

                    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); //Obtener el id de la sesión actual

                    solicitudEncontrada.IdTipoSolicitud = flsolicitud.IdTipoSolicitud;
                    solicitudEncontrada.IdArea = flsolicitud.IdArea;
                    solicitudEncontrada.Comentarios = flsolicitud.Comentarios;
                    await _context.SaveChangesAsync();

                    //Agregamos el nuevo detalle
                    foreach (var item in flsolicitud.detalle)
                    {
                        var idSolicitud = solicitudEncontrada.IdSolicitud;
                        var idProducto = item.idProducto;
                        var cantidad = item.cantidad;
                        var idMaquina = item.idMaquina;

                        var filasAfectadas = await _context.Database.ExecuteSqlRawAsync(
                            "EXEC [dbo].[spAgregarSolicitudDet] @IdSolicitud, @IdProducto, @cantidad, @IdMaquina",
                            new SqlParameter("@IdSolicitud", idSolicitud),
                            new SqlParameter("@IdProducto", idProducto),
                            new SqlParameter("@cantidad", cantidad),
                            new SqlParameter("@IdMaquina", idMaquina)
                        );
                    }

                    TempData["guardado"] = "Registro con id: "+solicitudEncontrada.IdSolicitud+" editado correctamente";
                    return RedirectToAction(nameof(Index));
            }

            var areas = _context.Flareas.ToList();
            var tipoSolicitud = _context.FltipoSolicituds
                .ToList();
            var estados = _context.Flstatuses.ToList();
            var maquinas = _context.Flmaquinas.ToList();

            var tipoSolicitudes = new List<TipoSolicitudViewModel>();
            foreach (var solicitud in tipoSolicitud)
            {
                var flujo = _context.Flflujos.Find(solicitud.IdFlujo);
                var mostrarMaquina = flujo.SeeMaquina == 1 ? true : false;

                tipoSolicitudes.Add(new TipoSolicitudViewModel(solicitud, mostrarMaquina));
            }

            var solicitudVm = new SolicitudViewModel();
            solicitudVm.Areas = new SelectList(areas, "IdArea", "NombreArea");
            solicitudVm.Estados = new SelectList(estados, "IdStatus", "NombreStatus");
            solicitudVm.TiposSolicitud = tipoSolicitudes;
            solicitudVm.Maquinas = new SelectList(maquinas, "IdMaquina", "NombreMaquina");
            return View(solicitudVm);
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


        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<JsonResult> Recibio([FromBody] int id)
        {
            //id de la solicitud
            await _context.Database.ExecuteSqlRawAsync("EXEC sp_RecibidoSolicitante @p0", id); //Ejecuta el sp de recibioSolcitiante
            //Ya que no devuvle ninguna tala, no se realizo un modelo para la tabla, de lo contrario hay que hacerlo

            return Json(new { success = true, message = "Estado de solicitud: "+id+" actualizado correctamente." });
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<JsonResult> Autorizar([FromBody] int id)
        {
            //id de la solicitud
            var solicitud = await _context.Flsolicituds
                .FindAsync(id); //Buscamos la solicuitud para obtener el di del estado

            await _context.Database.ExecuteSqlRawAsync("EXEC spAutorizacion @IdSolicitud = {0}, @Estatus = {1}",
                id,solicitud.IdStatus); //Ejecuta el sp de spAutorizacion

            return Json(new { success = true, message = "Solicitud: "+id+" autorizada correctamente." });
        }

        [HttpGet("busquedaItem")]
        public IActionResult busquedaItem(string term, int page = 1, int pageSize = 10)
        {
            try
            {
                // Simula una búsqueda en base de datos
                var productos = _context.VwProductos2s
                    .Where(u => string.IsNullOrEmpty(term) ||
                               u.Codigo.Contains(term) ||
                               u.ItemName.Contains(term))
                    .Select(u => new {
                        id = u.Id,
                        text = u.ItemName,
                        cantidad = u.Existencia
                    })
                    .ToList();

                var totalCount = _context.VwProductos2s
                    .Count(u => string.IsNullOrEmpty(term) ||
                               u.Codigo.Contains(term) ||
                               u.ItemName.Contains(term));

                // Formato esperado por Select2
                var result = new
                {
                    results = productos,
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
    }
}
