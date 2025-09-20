using ArquitecturaModel;
using ArquitecturaModel.Model;
using ArquitecturaModel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Confiteria.Controllers
{
    //[Authorize(Roles = "Admin")]
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
                        View(await _context.Productos.Include(i=> i.Marcas).ToListAsync()) :
                        Problem("Entity set 'AplicationDbContext.Productos'  is null.");
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var productos = await _context.Productos.Include(i => i.Marcas)
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
            var tazaId = _context.TasaDolar.Max(m => m.Id);
            return Json(_context.TasaDolar.Find(tazaId));
        }

        [HttpGet]
        public IActionResult GetAllProducto()
        {
            return Ok( _context.Productos.Include(i => i.Marcas).ToList());
        }
        // GET: Clientes/Create
        public IActionResult Create()
        {
            ViewData["marcasId"] = new SelectList(_context.Marcas, "Id", "Descripcion");
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
            ViewData["marcasId"] = new SelectList(_context.Marcas, "Id", "Descripcion", productos.MarcasId);
            var UsuarioId = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            var p = new Productos() {
                PrecioCosto= Convert.ToDecimal(productos.PrecioCosto.Replace(",", ".")),
                Precio = Convert.ToDecimal(productos.Precio.Replace(",",".")),
                PrecioDolar = Convert.ToDecimal(productos.PrecioDolar.Replace(",", ".")),
                //Stock = Convert.ToInt32(productos.Stock.ToString()),
                Codigo = productos.Codigo,
                Descripcion = productos.Descripcion,
                Fecha = DateTime.Now,
                MarcasId = productos.MarcasId
            };
			if (_context.Productos.Any(a => a.Codigo == productos.Codigo))
			{
				ModelState.AddModelError(nameof(productos.Codigo), $"El Codigo {productos.Codigo} ya existe.!");
				return View(productos);
			}

			if (ModelState.IsValid)
            {
                _context.Add(p);
                await _context.SaveChangesAsync();
                await _context.Inventario.AddAsync(new Inventario
                {
                    ProductosId = p.Id,
                    Stock = Convert.ToInt32(productos.Stock.ToString()),
                    SucursalId = UsuarioId!.SucursalId
                });
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
            var UsuarioId = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            var V = new ProductoViewModel()
            {
                ProductoId= productos.Id,
                Codigo = productos.Codigo,
                Descripcion = productos.Descripcion,
                Stock =  Convert.ToInt32(_context.Inventario!.FirstOrDefault(f=> f.SucursalId == UsuarioId.SucursalId && f.ProductosId == productos.Id)!.Stock),
                PrecioCosto= productos.PrecioCosto.ToString(),
                Precio = productos.Precio.ToString(),
                PrecioDolar = productos.PrecioDolar.ToString(),
                MarcasId = productos.MarcasId,
                SucursalId = UsuarioId!.SucursalId
            };
            ViewData["marcasId"] = new SelectList(_context.Marcas, "Id", "Descripcion", productos.MarcasId);
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
            ViewData["marcasId"] = new SelectList(_context.Marcas, "Id", "Descripcion", productos.MarcasId);
            var p = new Productos()
            {
                Id = productos.ProductoId,
                PrecioCosto = Convert.ToDecimal(productos.PrecioCosto.Replace(",", ".")),
                Precio = Convert.ToDecimal(productos.Precio.Replace(",",".")),
                PrecioDolar = Convert.ToDecimal(productos.PrecioDolar.Replace(",", ".")),
                //Stock = Convert.ToInt32(productos.Stock.ToString()),
                Codigo = productos.Codigo,
                Descripcion = productos.Descripcion,
                Fecha=DateTime.Now,
                MarcasId = productos.MarcasId
            };
            if (ModelState.IsValid)
            {
                using (var tr = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Update(p);
                        _context.Inventario.Update(new Inventario
                        {
                            ProductosId = p.Id,
                            Stock = Convert.ToInt32(productos.Stock.ToString()),
                            SucursalId = productos.SucursalId
                        });
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

            var productos = await _context.Productos.Include(i => i.Marcas)
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
