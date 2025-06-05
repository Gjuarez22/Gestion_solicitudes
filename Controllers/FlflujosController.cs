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
    public class FlflujosController : Controller
    {
        private readonly DbFlujosTestContext _context;

        public FlflujosController(DbFlujosTestContext context)
        {
            _context = context;
        }

        // GET: Flflujos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Flflujos.ToListAsync());
        }

        // GET: Flflujos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flflujo = await _context.Flflujos
                .FirstOrDefaultAsync(m => m.IdFlujo == id);
            if (flflujo == null)
            {
                return NotFound();
            }

            return View(flflujo);
        }

        // GET: Flflujos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flflujos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFlujo,DescripcionFlujo,HorasReintento,Activo")] Flflujo flflujo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flflujo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flflujo);
        }

        // GET: Flflujos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flflujo = await _context.Flflujos.FindAsync(id);
            if (flflujo == null)
            {
                return NotFound();
            }
            return View(flflujo);
        }

        // POST: Flflujos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFlujo,DescripcionFlujo,HorasReintento,Activo")] Flflujo flflujo)
        {
            if (id != flflujo.IdFlujo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flflujo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlflujoExists(flflujo.IdFlujo))
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
            return View(flflujo);
        }

        // GET: Flflujos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flflujo = await _context.Flflujos
                .FirstOrDefaultAsync(m => m.IdFlujo == id);
            if (flflujo == null)
            {
                return NotFound();
            }

            return View(flflujo);
        }

        // POST: Flflujos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flflujo = await _context.Flflujos.FindAsync(id);
            if (flflujo != null)
            {
                _context.Flflujos.Remove(flflujo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlflujoExists(int id)
        {
            return _context.Flflujos.Any(e => e.IdFlujo == id);
        }
    }
}
