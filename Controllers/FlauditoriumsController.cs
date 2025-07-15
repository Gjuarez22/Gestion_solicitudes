using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionSolicitud.Models;
using Microsoft.AspNetCore.Authorization;

namespace GestionSolicitud.Controllers
{
    [Authorize]
    public class FlauditoriumsController : Controller
    {
        private readonly DbFlujosTestContext _context;

        public FlauditoriumsController(DbFlujosTestContext context)
        {
            _context = context;
        }

        // GET: Flauditoriums
        public async Task<IActionResult> Index()
        {
            return View(await _context.Flauditoria.ToListAsync());
        }

        // GET: Flauditoriums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flauditorium = await _context.Flauditoria
                .FirstOrDefaultAsync(m => m.IdAuditoria == id);

            if (flauditorium == null)
            {
                return NotFound();
            }

            return View(flauditorium);
        }

        // GET: Flauditoriums/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flauditoriums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAuditoria,IdSolicitud,IdStatus,Fecha,IdUsuario,Notas")] Flauditorium flauditorium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flauditorium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flauditorium);
        }

        // GET: Flauditoriums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flauditorium = await _context.Flauditoria.FindAsync(id);
            if (flauditorium == null)
            {
                return NotFound();
            }
            return View(flauditorium);
        }

        // POST: Flauditoriums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAuditoria,IdSolicitud,IdStatus,Fecha,IdUsuario,Notas")] Flauditorium flauditorium)
        {
            if (id != flauditorium.IdAuditoria)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flauditorium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlauditoriumExists(flauditorium.IdAuditoria))
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
            return View(flauditorium);
        }

        // GET: Flauditoriums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flauditorium = await _context.Flauditoria
                .FirstOrDefaultAsync(m => m.IdAuditoria == id);
            if (flauditorium == null)
            {
                return NotFound();
            }

            return View(flauditorium);
        }

        // POST: Flauditoriums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flauditorium = await _context.Flauditoria.FindAsync(id);
            if (flauditorium != null)
            {
                _context.Flauditoria.Remove(flauditorium);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlauditoriumExists(int id)
        {
            return _context.Flauditoria.Any(e => e.IdAuditoria == id);
        }

       // [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DetallesPorSolicitud(int? id)
        {
            //Id debe de ser rl id de la solicitud
            if (id == null)
            {
                return NotFound();
            }

            /*var flauditorium = await _context.Flauditoria
                .Where(x=>x.IdSolicitud == id)
                .FirstOrDefaultAsync();*/

             var flauditorium = await _context.Flauditoria
                 .Where(x => x.IdSolicitud == id)
                 .ToListAsync();   
            

            foreach (var item in flauditorium)
            {
                item.Estatus = _context.Flstatuses
                    .Where(s => s.IdStatus == item.IdStatus)
                    .Select(s => s.NombreStatus)
                    .FirstOrDefault();

                item.Ususario_ = _context.Flusuarios
                    .Where(s => s.IdUsuario == item.IdUsuario)
                    .Select(s => s.Nombre)
                    .FirstOrDefault();
            }

            /*foreach (var item in flauditorium)
            {
                item.Ususario_ = _context.Flusuarios
                    .Where(s => s.IdUsuario == item.IdUsuario)
                    .Select(s => s.Nombre)
                    .FirstOrDefault();
            }*/


            if (flauditorium == null || !flauditorium.Any())
            {
                return NotFound();
            }
            /*else
            {
                ViewBag.usuario = _context.Flusuarios.Find(flauditorium.IdUsuario);
                ViewBag.solicitud  = _context.Flsolicituds.Find(flauditorium.IdSolicitud);
                ViewBag.estado = _context.Flstatuses.Find(flauditorium.IdStatus);
                   
            }*/

         

            return PartialView(flauditorium);
        }
    }
}
