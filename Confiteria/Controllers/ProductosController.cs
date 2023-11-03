using ArquitecturaModel.Model;
using ArquitecturaModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ArquitecturaModel.ViewModels;

namespace Confiteria.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly AplicationDbContext _context;

        public ProductosController(AplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return _context.Productos != null ?
                        View(await _context.Productos.ToListAsync()) :
                        Problem("Entity set 'AplicationDbContext.Productos'  is null.");
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var productos = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productos == null)
            {
                return NotFound();
            }

            return View(productos);
        }

        [HttpGet]
        public IActionResult GetTaza()
        {
            return Json(_context.TasaDolars.FirstOrDefault());
        }
        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoViewModel productos)
        {
            //productos.Fecha = DateTime.Now;
            //productos.Imagen = "Watson Watson";

            var p = new Productos() {
                Precio = Convert.ToDouble(productos.Precio),
                PrecioDolar = Convert.ToDouble(productos.PrecioDolar),
                Stock = Convert.ToInt32(productos.Stock.ToString()),
                Codigo = productos.Codigo,
                Descripcion = productos.Descripcion,
                Fecha = DateTime.Now,
                
            };
            if (ModelState.IsValid)
            {
                _context.Add(p);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productos);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var productos = await _context.Productos.FindAsync(id);
            if (productos == null)
            {
                return NotFound();
            }
            var V = new ProductoViewModel()
            {
                ProductoId= productos.Id,
                Codigo = productos.Codigo,
                Descripcion = productos.Descripcion,
                Stock = Convert.ToInt32(productos.Stock.ToString()),
                Precio = productos.Precio.ToString(),
                PrecioDolar = productos.PrecioDolar.ToString(),
            };
            return View(V);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductoViewModel productos)
        {
            if (id != productos.ProductoId)
            {
                return NotFound();
            }
            var p = new Productos()
            {
                Id = productos.ProductoId,
                Precio = Convert.ToDouble(productos.Precio),
                PrecioDolar = Convert.ToDouble(productos.PrecioDolar),
                Stock = Convert.ToInt32(productos.Stock.ToString()),
                Codigo = productos.Codigo,
                Descripcion = productos.Descripcion,
                Fecha=DateTime.Now,

            };
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductosExists(productos.ProductoId))
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
            return View(productos);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var productos = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productos == null)
            {
                return NotFound();
            }

            return View(productos);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Productos == null)
            {
                return Problem("Entity set 'AplicationDbContext.Productos'  is null.");
            }
            var productos = await _context.Productos.FindAsync(id);
            if (productos != null)
            {
                _context.Productos.Remove(productos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductosExists(int id)
        {
            return (_context.Productos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
