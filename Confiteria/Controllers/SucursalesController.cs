using ArquitecturaModel;
using ArquitecturaModel.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Confiteria.Controllers
{
	public class SucursalesController : Controller
	{
		private readonly AplicationDbContext _context;

		public SucursalesController(AplicationDbContext context)
		{
			_context = context;
		}

		// GET: SucursalesController
		public async Task<IActionResult> Index()
		{
			return _context.Sucursales != null ?
						View(await _context.Sucursales.ToListAsync()) :
						Problem("Entity set 'AplicationDbContext.Sucursales'  is null.");
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Sucursales == null)
			{
				return NotFound();
			}

			var sucursales = await _context.Sucursales
				.FirstOrDefaultAsync(m => m.SucursalId == id);
			if (sucursales == null)
			{
				return NotFound();
			}

			return View(sucursales);
		}

		// GET: SucursalesController/Create
		public IActionResult Create()
		{
			ViewData["TipoDocumentoId"] = new SelectList(_context.TipoDocumentos, "Documento", "Documento");
			return View();
		}

		// POST: SucursalesController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Sucursales sucursales)
		{
			sucursales.Fecha = DateTime.Now;
			ViewData["TipoDocumentoId"] = new SelectList(_context.TipoDocumentos, "Documento", "Documento", sucursales.TipoDocumento);
			if (_context.Clientes.Any(a => a.TipoDocumento == sucursales.TipoDocumento && a.Rif == sucursales.Rif))
			{
				ModelState.AddModelError(nameof(sucursales.Rif), $"El Rif {sucursales.Rif} ya existe.!");
				return View(sucursales);
			}
			if (ModelState.IsValid)
			{
				try
				{
					_context.Add(sucursales);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				catch (Exception exception)
				{
					ModelState.AddModelError(string.Empty, exception.Message);
				}
			}
			return View(sucursales);
		}
		// GET: SucursalesController/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Sucursales == null)
			{
				return NotFound();
			}

			var sucursales = await _context.Sucursales.FindAsync(id);
			if (sucursales == null)
			{
				return NotFound();
			}
			ViewData["TipoDocumentoId"] = new SelectList(_context.TipoDocumentos, "Documento", "Documento");
			return View(sucursales);
		}
		// POST: SucursalesController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Sucursales sucursales)
		{
			sucursales.Fecha = DateTime.Now;
			ViewData["TipoDocumentoId"] = new SelectList(_context.TipoDocumentos, "Documento", "Documento", sucursales.TipoDocumento);
			if (id != sucursales.SucursalId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(sucursales);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!SucursalExists(sucursales.SucursalId))
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
			return View(sucursales);
		}


		// GET: SucursalesController/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Sucursales == null)
			{
				return NotFound();
			}

			var sucursales = await _context.Sucursales
				.FirstOrDefaultAsync(m => m.SucursalId == id);
			if (sucursales == null)
			{
				return NotFound();
			}

			return View(sucursales);
		}

		// POST: SucursalesController/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Sucursales == null)
			{
				return Problem("Entity set 'AplicationDbContext.Sucursales'  is null.");
			}
			var sucursal = await _context.Sucursales.FindAsync(id);
			if (sucursal != null)
			{
				_context.Sucursales.Remove(sucursal);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		private bool SucursalExists(int id)
		{
			return (_context.Sucursales?.Any(e => e.SucursalId == id)).GetValueOrDefault();
		}
	}
}
