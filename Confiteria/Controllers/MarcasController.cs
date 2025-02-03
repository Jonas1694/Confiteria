using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArquitecturaModel.Model;
using Confiteria.Data;
using ArquitecturaModel;
using Microsoft.AspNetCore.Authorization;

namespace Confiteria.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MarcasController : Controller
    {
        private readonly AplicationDbContext _context;

        public MarcasController(AplicationDbContext context)
        {
            _context = context;
        }

        // GET: Marcas
        public async Task<IActionResult> Index()
        {
              return _context.Marcas != null ? 
                          View(await _context.Marcas.ToListAsync()) :
                          Problem("Entity set 'ConfiteriaContext.Marcas'  is null.");
        }

        // GET: Marcas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Marcas == null)
            {
                return NotFound();
            }

            var marcas = await _context.Marcas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marcas == null)
            {
                return NotFound();
            }

            return View(marcas);
        }

        // GET: Marcas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Marcas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Marcas marcas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(marcas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(marcas);
        }

        // GET: Marcas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Marcas == null)
            {
                return NotFound();
            }

            var marcas = await _context.Marcas.FindAsync(id);
            if (marcas == null)
            {
                return NotFound();
            }
            return View(marcas);
        }

        // POST: Marcas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Marcas marcas)
        {
            if (id != marcas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(marcas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarcasExists(marcas.Id))
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
            return View(marcas);
        }

        // GET: Marcas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Marcas == null)
            {
                return NotFound();
            }

            var marcas = await _context.Marcas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marcas == null)
            {
                return NotFound();
            }

            return View(marcas);
        }

        // POST: Marcas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Marcas == null)
            {
                return Problem("Entity set 'ConfiteriaContext.Marcas'  is null.");
            }
            var marcas = await _context.Marcas.FindAsync(id);
            if (marcas != null)
            {
                _context.Marcas.Remove(marcas);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarcasExists(int id)
        {
          return (_context.Marcas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
