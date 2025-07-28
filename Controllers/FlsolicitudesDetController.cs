using GestionSolicitud.Models;
using GestionSolicitud.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAPbobsCOM;
using System.Data;
using System.Security.Claims;

namespace GestionSolicitud.Controllers
{
    [Authorize]
    public class FlsolicitudesDetController : Controller
    {
        private readonly DbFlujosTestContext _context;

        public FlsolicitudesDetController(DbFlujosTestContext context)
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
                    detalle.Umedida = producto?.UniMed ?? "(Unidad no encontrada)";
                }

            }


            return PartialView(flsolicitud);
        }



        // GET: FlsolicitudesDet/Create
        public IActionResult Create(int id)
        {

            var model = new SolicitudDetalleViewModel
            {
                IdSolicitud = id,
                Maquina = ObtenerMaquinas() // Método que te devuelve un SelectList
            };

            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SolicitudViewModel flsolicitud)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); //Obtener el id de la sesión actual

                var solicitud = new Flsolicitud();
                solicitud.Fecha = DateTime.Now;
                solicitud.IdSolicitante = userId;
                solicitud.IdTipoSolicitud = flsolicitud.IdTipoSolicitud;
                solicitud.IdStatus = "PRE";
                solicitud.DocNumErp = null;
                solicitud.Comentarios = flsolicitud.Comentarios;
                solicitud.IdArea = flsolicitud.IdArea;
                solicitud.Cancelada = false;

                _context.Add(solicitud);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CreateDetalles));
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

            return Json(new { success = true, message = "Estado de solicitud: " + id + " actualizado correctamente." });
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<JsonResult> Autorizar([FromBody] int id)
        {
            //id de la solicitud
            var solicitud = await _context.Flsolicituds
                .FindAsync(id); //Buscamos la solicuitud para obtener el di del estado

            await _context.Database.ExecuteSqlRawAsync("EXEC spAutorizacion @IdSolicitud = {0}, @Estatus = {1}",
                id, solicitud.IdStatus); //Ejecuta el sp de spAutorizacion

            return Json(new { success = true, message = "Solicitud: " + id + " autorizada correctamente." });
        }

        public IActionResult CreateDetalles()
        {
            return View();
        }

        private SelectList ObtenerMaquinas()
        {
            var maquinas = _context.Flmaquinas
                .Select(m => new { m.IdMaquina, m.NombreMaquina })
                .ToList();

            return new SelectList(maquinas, "IdMaquina", "NombreMaquina");
        }

        //[HttpGet]
        //public JsonResult BusquedaItem(string buscar, string almacen)
        //{
        //    var resultados = _context.VwProductos
        //        .FromSqlRaw("EXEC sp_BuscarItemAutocompleatado @p0, @p1", buscar, almacen)
        //        .AsNoTracking()
        //        .ToList();

        //    return Json(resultados);
        //}
    }
}
