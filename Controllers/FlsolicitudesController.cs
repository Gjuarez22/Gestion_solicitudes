using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionSolicitud.Models;
using System.Security.Claims;

namespace GestionSolicitud.Controllers
{
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
            int.TryParse(claimRol, out int idRol);

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
                .FirstOrDefaultAsync(m => m.IdSolicitud == id);
            if (flsolicitud == null)
            {
                return NotFound();
            }

            return View(flsolicitud);
        }

        // GET: Flsolicitudes/Create
        public IActionResult Create()
        {
            ViewData["IdArea"] = new SelectList(_context.Flareas, "IdArea", "IdArea");
            ViewData["IdSolicitante"] = new SelectList(_context.Flusuarios, "IdUsuario", "IdUsuario");
            ViewData["IdStatus"] = new SelectList(_context.Flstatuses, "IdStatus", "IdStatus");
            ViewData["IdTipoSolicitud"] = new SelectList(_context.FltipoSolicituds, "IdTipoSolicitud", "IdTipoSolicitud");
            return View();
        }

        // POST: Flsolicitudes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSolicitud,Fecha,IdSolicitante,IdTipoSolicitud,IdArea,IdStatus,DocNumErp,Comentarios,Cancelada,Reenviada")] Flsolicitud flsolicitud)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flsolicitud);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdArea"] = new SelectList(_context.Flareas, "IdArea", "IdArea", flsolicitud.IdArea);
            ViewData["IdSolicitante"] = new SelectList(_context.Flusuarios, "IdUsuario", "IdUsuario", flsolicitud.IdSolicitante);
            ViewData["IdStatus"] = new SelectList(_context.Flstatuses, "IdStatus", "IdStatus", flsolicitud.IdStatus);
            ViewData["IdTipoSolicitud"] = new SelectList(_context.FltipoSolicituds, "IdTipoSolicitud", "IdTipoSolicitud", flsolicitud.IdTipoSolicitud);
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
    }
}
