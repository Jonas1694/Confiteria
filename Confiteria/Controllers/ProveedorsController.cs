using ArquitecturaModel.Model;
using ArquitecturaModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Confiteria.Controllers
{
    public class ProveedorsController : Controller
    {
        private readonly AplicationDbContext _context;

        public ProveedorsController(AplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return _context.Proveedors != null ?
                        View(await _context.Proveedors.ToListAsync()) :
                        Problem("Entity set 'AplicationDbContext.Clientes'  is null.");
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Proveedors == null)
            {
                return NotFound();
            }

            var cliente = await _context.Proveedors
                .FirstOrDefaultAsync(m => m.ProveedorId == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
			ViewData["TipoDocumentoId"] = new SelectList(_context.TipoDocumentos, "Documento", "Documento");
			return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proveedor proveedor)
        {
			ViewData["TipoDocumentoId"] = new SelectList(_context.TipoDocumentos, "Documento", "Documento");
			if (_context.Proveedors.Any(a => a.TipoDocumento == proveedor.TipoDocumento && a.Rif == proveedor.Rif))
			{
				ModelState.AddModelError(nameof(proveedor.Rif), $"El Rif {proveedor.Rif} ya existe.!");
				return View(proveedor);
			}
			if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Proveedors == null)
            {
                return NotFound();
            }

            var cliente = await _context.Proveedors.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
			ViewData["TipoDocumentoId"] = new SelectList(_context.TipoDocumentos, "Documento", "Documento", cliente.TipoDocumento);
			return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Proveedor proveedor)
        {
			ViewData["TipoDocumentoId"] = new SelectList(_context.TipoDocumentos, "Documento", "Documento", proveedor.TipoDocumento);
			if (id != proveedor.ProveedorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorsExists(proveedor.ProveedorId))
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
            return View(proveedor);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Proveedors == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedors
                .FirstOrDefaultAsync(m => m.ProveedorId == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Proveedors == null)
            {
                return Problem("Entity set 'AplicationDbContext.Proveedors'  is null.");
            }
            var proveedor = await _context.Proveedors.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedors.Remove(proveedor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedorsExists(int id)
        {
            return (_context.Proveedors?.Any(e => e.ProveedorId == id)).GetValueOrDefault();
        }
    }
}
