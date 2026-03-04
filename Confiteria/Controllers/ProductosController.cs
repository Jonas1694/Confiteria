using ArquitecturaModel;
using ArquitecturaModel.Model;
using ArquitecturaModel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
            List<Inventario> inventarios = new List<Inventario>();
            if(User.IsInRole("Admin") || User.IsInRole("sa"))
            {
                inventarios = await _context.Inventario
                    //.Include(i => i.Marcas)
                    .Include(i => i.Productos)
                    .Include(i=> i.Sucursales)
                    .Include(i=> i.Productos.Marcas)
                    .Include("Inventario.Sucursales")
                    .ToListAsync();
            }
            else
            {
                var UsuarioId = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
                inventarios = await _context.Inventario
                       //.Include(i => i.Marcas)
                       .Include(i => i.Productos)
                       .Include(i => i.Sucursales)
                       .Include(i => i.Productos.Marcas)
                       .Include("Inventario.Sucursales")
                       .Where(w => w.SucursalesId == UsuarioId!.SucursalesId)
                       .ToListAsync();
            }
            var productos = inventarios
                .Select(s => new GridProductoVewModel()
            {
                Id = s.Productos.Id,
                Codigo = s.Productos.Codigo,
                Descripcion = s.Productos.Descripcion,
                Fecha = s.Productos.Fecha,
                Precio = s.Productos.Precio,
                Stock = s.Stock,
                Marcas = s.Productos.Marcas,
                SucursalName = s.Sucursales.SucursalName
            }).ToList();
            return productos != null ? View(productos) : Problem("Entity set 'AplicationDbContext.Productos'  is null.");
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var productos = await _context.Productos.Include(i => i.Marcas)
                .Include(i => i.Inventario)
                .FirstOrDefaultAsync(m => m.Id == id);
            var sucursal = _context.Sucursales.ToList();
            foreach (var item in productos.Inventario)
            {
                item.Sucursales = sucursal.FirstOrDefault(f => f.SucursalId == item.SucursalesId);
            }
            if (productos == null)
            {
                return NotFound();
            }

            return View(productos);
        }

        [HttpGet]
        public IActionResult GetTaza()
        {
            var taza = _context.TasaDolar.ToList();
            if (taza.Count == 0)
            {
                return Json(new { Id = 0, Valor = 0 });
            }
            var tazaId = taza.Max(m => m.Id);  
            return Json(_context.TasaDolar.Find(tazaId));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducto()
        {
            List<Inventario> inventarios = new List<Inventario>();
            inventarios = await _context.Inventario
                   //.Include(i => i.Marcas)
                   .Include(i => i.Productos)
                   .Include(i => i.Sucursales)
                   .Include(i => i.Productos.Marcas)
                   .Include("Inventario.Sucursales")
                   .ToListAsync();
            var productos = inventarios
               .Select(s => new GridProductoVewModel()
               {
                   Id = s.Productos.Id,
                   Codigo = s.Productos.Codigo,
                   Descripcion = s.Productos.Descripcion,
                   Fecha = s.Productos.Fecha,
                   Precio = s.Productos.Precio,
                   Stock = s.Stock,
                   Marcas = s.Productos.Marcas,
                   SucursalName = s.Sucursales.SucursalName
               }).ToList();
            return Ok(productos);
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
            var p = new Productos()
            {
                PrecioCosto = Convert.ToDecimal(productos.PrecioCosto.Replace(",", ".")),
                Precio = Convert.ToDecimal(productos.Precio.Replace(",", ".")),
                PrecioDolar = Convert.ToDecimal(productos.PrecioDolar.Replace(",", ".")),
				Precio2 = Convert.ToDecimal(productos.Precio2.Replace(",", ".")),
				PrecioDolar2 = Convert.ToDecimal(productos.PrecioDolar2.Replace(",", ".")),
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
                using (var tr = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Productos.Add(p);
                        await _context.SaveChangesAsync();
                        var inventario = new Inventario
                        {
                            ProductosId = p.Id,
                            Stock = Convert.ToInt32(productos.Stock.ToString()),
                            SucursalesId = UsuarioId!.SucursalesId!.Value
                        };
                        _context.Inventario.Add(inventario);
                        await _context.SaveChangesAsync();
                        tr.Commit();
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                    }
                }
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
                Id = productos.Id,
                Codigo = productos.Codigo,
                Descripcion = productos.Descripcion,
                Stock = _context.Inventario!.Any(f => f.SucursalesId == UsuarioId.SucursalesId && f.ProductosId == productos.Id) ? _context.Inventario!.FirstOrDefault(f => f.SucursalesId == UsuarioId.SucursalesId && f.ProductosId == productos.Id)!.Stock : 0,
                PrecioCosto = productos.PrecioCosto.ToString(),
                Precio = productos.Precio.ToString(),
                PrecioDolar = productos.PrecioDolar.ToString(),
				Precio2 = productos.Precio2.ToString(),
				PrecioDolar2 = productos.PrecioDolar2.ToString(),
				MarcasId = productos.MarcasId,
                SucursalId = UsuarioId!.SucursalesId!.Value
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
            if (id != productos.Id)
            {
                return NotFound();
            }
            ViewData["marcasId"] = new SelectList(_context.Marcas, "Id", "Descripcion", productos.MarcasId);
            var p = new Productos()
            {
                Id = productos.Id,
                PrecioCosto = Convert.ToDecimal(productos.PrecioCosto.Replace(",", ".")),
                Precio = Convert.ToDecimal(productos.Precio.Replace(",", ".")),
                PrecioDolar = Convert.ToDecimal(productos.PrecioDolar.Replace(",", ".")),
				Precio2 = Convert.ToDecimal(productos.Precio2.Replace(",", ".")),
				PrecioDolar2 = Convert.ToDecimal(productos.PrecioDolar2.Replace(",", ".")),
				//Stock = Convert.ToInt32(productos.Stock.ToString()),
				Codigo = productos.Codigo,
                Descripcion = productos.Descripcion,
                Fecha = DateTime.Now,
                MarcasId = productos.MarcasId
            };
            if (ModelState.IsValid)
            {
                using (var tr = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Productos.Update(p);
                        await _context.SaveChangesAsync();
                        if (_context.Inventario.Any(a => a.SucursalesId == productos.SucursalId && a.ProductosId == p.Id))
                        {
                            var invetario = _context.Inventario.FirstOrDefault(a => a.SucursalesId == productos.SucursalId && a.ProductosId == p.Id);
                            invetario.Stock = Convert.ToInt32(productos.Stock.ToString());
                            _context.Inventario.Update(invetario);
                        }
                        else
                        {
                            var inventario = new Inventario
                            {
                                ProductosId = p.Id,
                                Stock = Convert.ToInt32(productos.Stock.ToString()),
                                SucursalesId = productos.SucursalId
                            };
                            await _context.Inventario.AddAsync(inventario);
                        }
                        await _context.SaveChangesAsync();
                        tr.Commit();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        tr.Rollback();
                        if (!ProductosExists(productos.Id))
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
               .Include(i => i.Inventario)
               .FirstOrDefaultAsync(m => m.Id == id);
            var sucursal = _context.Sucursales.ToList();
            foreach (var item in productos.Inventario)
            {
                item.Sucursales = sucursal.FirstOrDefault(f => f.SucursalId == item.SucursalesId);
            }
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
