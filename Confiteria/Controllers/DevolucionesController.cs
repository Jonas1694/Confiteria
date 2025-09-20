using ArquitecturaModel;
using ArquitecturaModel.Model;
using ArquitecturaModel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Confiteria.Controllers
{
    public class DevolucionesController : Controller
    {
        private readonly AplicationDbContext _context;

        public DevolucionesController(AplicationDbContext context)
        {
            _context = context;
        }
        // GET: Devoluciones
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Devolucions.Include(d => d.Clientes).Include(d => d.StatusDocumento);
            return View(await applicationDbContext.ToListAsync());
        }
        // GET: Devoluciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devolucions
                .Include(d => d.Clientes)
                .Include(d => d.StatusDocumento)
                .FirstOrDefaultAsync(m => m.DevolucionId == id);
            if (devolucion == null)
            {
                return NotFound();
            }
            var model = new DevolucionViewModel();

            return View(model.ReturnViewModel(devolucion, _context.DetalleDevoluciones.Include(i => i.Producto).Include(i => i.Producto.Marcas).Where(w => w.DocumentoId == id).ToList()));
        }
        // GET: Devoluciones/Create
        public async Task<IActionResult> Create(int? id)
        {

            if (id != null)
            {
                var doc = _context.Facturacion.Include(i => i.Clientes).Where(c => c.FacturacionId == id.Value).FirstOrDefault();
                var detalle = _context.DetalleFacturas.Include(i => i.Productos).Include(i => i.Productos).Include(i => i.Productos.Marcas).Where(d => d.FacturacionId == doc.FacturacionId);


                List<DetalleDevolucionViewModel> List = new List<DetalleDevolucionViewModel>();
                foreach (var item in detalle)
                {
                    List.Add(new DetalleDevolucionViewModel
                    {
                        Cantidad = item.Cantidad,
                        Iva = item.Iva,
                        IvaUnitario = item.IvaUnitario,
                        PrecioUnitario = item.PrecioUnitario,
                        Producto = item.Productos.Descripcion,
                        ProductoId = item.ProductosId,
                        SubTotal = item.SubTotal,
                        Total = item.Total,
                        Tasa=item.Facturacion.Tasa,
                        TotalIva = item.TotalIva
                    });
                }
                var dev = new DevolucionViewModel()
                {
                    ClienteId = doc.ClienteId,
                    FacturaId = doc.FacturacionId,
                    Iva = doc.Iva,
                    NDocumento = _context.Devolucions.Count() + 1,
                    SubTotal = doc.SubTotal,
                    Total = doc.Total,
                    Tasa= doc.Tasa,
                    TotalIva = doc.TotalIva,
                    DetalleDocumentoViews = List
                };
                ViewData["ClienteId"] = new SelectList(await _context.Clientes.ToListAsync(), "id", "GetRif");
                ViewData["ProductosId"] = new SelectList(await _context.Productos.ToListAsync(), "Id", "GetDescripcion");

                return View(dev);
            }
            ViewData["ClienteId"] = new SelectList(await _context.Clientes.ToListAsync(), "id", "GetRif");
            ViewData["ProductosId"] = new SelectList(await _context.Productos.ToListAsync(), "Id", "GetDescripcion");
           
            return View(new DevolucionViewModel());
        }
        // POST: Devoluciones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DevolucionViewModel model, string action)
        {
			var tazaId = _context.TasaDolar.Max(m => m.Id);
			var t = _context.TasaDolar.Find(tazaId) ?? null;
            var UsuarioId = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            ViewData["ClienteId"] = new SelectList(await _context.Clientes.ToListAsync(), "id", "GetRif");
            ViewData["ProductosId"] = new SelectList(await _context.Productos.ToListAsync(), "Id", "GetDescripcion");
            //ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "GetRif", model.ClienteId);
            //ViewData["ProductoId"] = new SelectList(_context.Productos.Include(i => i.Marcas), "Id", "GetDescripcion");
            switch (action)
            {
                case "addproducto":
                    if (model.Cantidad == 0)
                    {
                        ModelState.AddModelError("", "Debe introducir una cantidad");
                        return View(model);
                    }
                    return View(model.AddItems(model));
                case "Eliminar":
                    model.SubTotal = model.DetalleDocumentoViews.Where(w => w.Eliminado == false).Sum(s => s.SubTotal);
                    model.TotalIva = model.SubTotal * (Convert.ToDecimal(model.FormatPorCentaje(0.16M)) / 100);
                    model.Total = model.SubTotal + model.TotalIva;
                    return View(model);
                case "Registrar Factura":
                    if (model.DetalleDocumentoViews == null)
                    {
                        ModelState.AddModelError("", "No se puede procesar, porque no hay producto en la lista");
                        return View(model);
                    }
                    model = model.ToModel(model);
                    var data = _context.Devolucions;
                    double ncorrelativo = 0;
                    if (data.Count() != 0)
                    {
                        ncorrelativo = data.Max(m => m.NDocumento) + 1;
                    }
                    else
                    {
                        ncorrelativo = 1;
                    }
                    model.NDocumento = ncorrelativo;
                    if (string.IsNullOrEmpty(model.DescripcionDevolucion.Trim()))
                    {
                        ModelState.AddModelError(nameof(model.DescripcionDevolucion), "De introducir la descripción de la devolución!");
                        return View(model);
                    }
                    
                    using (var trans = _context.Database.BeginTransaction())
                    {
                        try
                        {
							model.Tasa = (t == null || t.Tasa == 0) ? 0 : (decimal.Parse(model.Total.ToString()) / t.Tasa);
							var devolucion = new Devolucion
                            {
                                ClienteId = model.ClienteId,
                                FechaRegistro = DateTime.Now,
                                Iva = model.Iva,
                                NDocumento = model.NDocumento,
                                StatusDocumentoId = 3,
                                SubTotal = model.SubTotal,
                                Total = model.Total,
                                TotalIva = model.TotalIva,
                                UserId = UsuarioId.Id,
                                Tasa= model.Tasa,
                                DescripcionDevolucion = model.DescripcionDevolucion
                            };
                            _context.Add(devolucion);
                            await _context.SaveChangesAsync();

                            foreach (var item in model.DetalleDocumentoViews)
                            {
                                var detalle = new DetalleDevolucion
                                {
                                    Cantidad = item.Cantidad,
                                    UserId = UsuarioId.Id,
                                    FechaRegistro = DateTime.Now,
                                    DocumentoId = devolucion.DevolucionId,
                                    ProductoId = item.ProductoId,
                                    PrecioUnitario = item.PrecioUnitario,
                                    SubTotal = item.SubTotal,
                                    Iva = 16,
                                    IvaUnitario = item.IvaUnitario,
                                    TotalIva = item.TotalIva,
                                    Total = item.Total,
                                };
                                _context.Add(detalle);
                                await _context.SaveChangesAsync();

                                var producto = _context.Inventario.AsNoTracking().FirstOrDefault(p => p.Id == item.ProductoId && p.SucursalId == UsuarioId.SucursalId);
                                producto.Stock += item.Cantidad;
                                _context.Inventario.Update(producto);
                                await _context.SaveChangesAsync();
                                if (model.FacturaId > 0)
                                {
                                    var fac = _context.Facturacion.FirstOrDefault(f => f.FacturacionId == model.FacturaId);
                                    fac.StatusDocumentoId = 3;
                                    _context.Facturacion.Update(fac);
                                    await _context.SaveChangesAsync();
                                }
                            }
                            trans.Commit();
                            return RedirectToAction(nameof(Index));
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "GetRif", model.ClienteId);
                            ViewData["ProductoId"] = new SelectList(_context.Productos.Include(i => i.Marcas), "Id", "GetDescripcion", model.ProductoId);
                            return View(model);
                        }
                    }
            }

            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "GetRif", model.ClienteId);
            ViewData["ProductoId"] = new SelectList(_context.Productos.Include(i => i.Marcas), "Id", "GetDescripcion");
            return View(model);
        }
        public async Task<IActionResult> GetPrecio(int id)
        {
            var UsuarioId = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            var p = await _context.Productos.Include(i => i.Marcas).SingleOrDefaultAsync(t => t.Id == id);
            var inventario = await _context.Inventario.AsNoTracking().FirstOrDefaultAsync(f => f.ProductosId == id && f.SucursalId == UsuarioId!.SucursalId);
            var data = new ProductoViewModel { Descripcion = p.GetDescripcion, Precio = p.Precio.ToString(), Stock = inventario!.Stock, StockMin = p.StockMin, StockMax = p.StockMax };
            var settings = new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() };
            return Json(new { data = data }, settings);
        }
        // GET: Devoluciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devolucions.Include(i=> i.Clientes).FirstOrDefaultAsync(f=> f.DevolucionId == id);
            if (devolucion == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "GetRif", devolucion.ClienteId);
            ViewData["ProductoId"] = new SelectList(_context.Productos.Include(i => i.Marcas), "Id", "GetDescripcion");
            return View(devolucion);
        }
        // POST: Devoluciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Devolucion devolucion)
        {
            if (id != devolucion.DevolucionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(devolucion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DevolucionExists(devolucion.DevolucionId))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "GetRif", devolucion.ClienteId);
            ViewData["ProductoId"] = new SelectList(_context.Productos.Include(i => i.Marcas), "Id", "GetDescripcion");
            return View(devolucion);
        }
        // GET: Devoluciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devolucions
                .Include(d => d.Clientes)
                .Include(d => d.StatusDocumento)
                .FirstOrDefaultAsync(m => m.DevolucionId == id);
            if (devolucion == null)
            {
                return NotFound();
            }

            return View(devolucion);
        }

        // POST: Devoluciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var devolucion = await _context.Devolucions.FindAsync(id);
            _context.Devolucions.Remove(devolucion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DevolucionExists(int id)
        {
            return _context.Devolucions.Any(e => e.DevolucionId == id);
        }
    }
}
