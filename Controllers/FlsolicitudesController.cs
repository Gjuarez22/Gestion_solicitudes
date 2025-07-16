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

                tipoSolicitudes.Add(new TipoSolicitudViewModel(solicitud,mostrarMaquina));
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
        public async Task<IActionResult> Create( SolicitudViewModel flsolicitud)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); //Obtener el id de la sesión actual
                
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
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
                var usuarios = _context.Flusuarios
                    .Where(u => string.IsNullOrEmpty(term) ||
                               u.Nombre.Contains(term))
                    .Select(u => new {
                        id = u.IdUsuario,
                        text = u.Nombre,
                        cantidad = 2
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
    }
}
