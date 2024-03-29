﻿using ArquitecturaModel.Model;
using ArquitecturaModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using ArquitecturaModel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Rotativa.AspNetCore;
using Stimulsoft.Data.Extensions;

namespace Confiteria.Controllers
{
	[Authorize]
	public class FacturacionController : Controller
	{
		private readonly AplicationDbContext _context;

		public FacturacionController(AplicationDbContext context)
		{
			_context = context;
		}
		[HttpPost]
		public async Task<ActionResult> BuscarProductos(string codigo)
		{
			//var settings = new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() };
			var data = await _context.Productos.FirstOrDefaultAsync(f => f.Codigo == codigo);
			return Json(data);
		}
		// GET: Facturacion
		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.Facturacion.Include(f => f.Clientes)
				.OrderByDescending(n => n.NFactura);
			return View(await applicationDbContext.ToListAsync());
		}

		// GET: Facturacion/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var facturacion = await _context.Facturacion
				.Include(f => f.Clientes)
				.Include(d => d.DetalleFacturas)
				.FirstOrDefaultAsync(m => m.FacturacionId == id);
			if (facturacion == null)
			{
				return NotFound();
			}
			var detalle = await _context.DetalleFacturas.Include(i => i.Productos).Where(w => w.FacturacionId == facturacion.FacturacionId).ToListAsync();
			var view = new FacturacionViewModel();
			return View(view.ReturnViewModel(facturacion, detalle));
		}

		// GET: Facturacion/Create
		public async Task<IActionResult> Create(int? id)
		{
			ViewData["ClienteId"] = new SelectList(_context.Clientes, "id", "GetRif");
			ViewData["ProductosId"] = new SelectList(_context.Productos, "Id", "GetDescripcion");
			ViewData["SelectFormaPago"] = new SelectList(_context.FormaPago, "Id", "Name");
			return View(new FacturacionViewModel());
		}

		// POST: Facturacion/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(FacturacionViewModel model, string action)
		{
			var t = _context.TasaDolars.FirstOrDefault() ?? null;
			ViewData["ClienteId"] = new SelectList(_context.Clientes, "id", "GetRif", model.ClienteId);
			ViewData["ProductosId"] = new SelectList(_context.Productos, "Id", "GetDescripcion", model.ProductosId);
			ViewData["SelectFormaPago"] = new SelectList(_context.FormaPago, "Id", "Name",model.FormaPagoId);
			
			switch (action)
			{
				case "addproducto":
					if (model.Cantidad == 0)
					{
						ModelState.AddModelError("", "Debe introducir una cantidad");
						return View(model);
					}
					model.Tasa = t == null ? 0 : t.Tasa;
					var data_ = model.AddItems(model);
					ModelState.Clear();
					return View(data_);
				case "Eliminar":
					model.SubTotal = model.DetalleFacturacionViews.Where(w => w.Eliminado == false).Sum(s => s.SubTotal);
					//model.TotalIva = model.SubTotal * (Convert.ToDecimal(model.FormatPorCentaje(0.16M)) / 100);
					model.Total = model.SubTotal /*+ model.TotalIva*/;
					return View(model);
				case "Registrar Factura":
					if (model.DetalleFacturacionViews == null)
					{
						ModelState.AddModelError("", "No se puede procesar, porque no hay producto en la lista");
						return View(model);
					}
					model.Tasa = t.Tasa;
					model = model.ToModel(model);
					var data = _context.Facturacion;
					double ncorrelativo = 0;
					if (data.Count() != 0)
					{
						ncorrelativo = data.Max(m => m.NFactura) + 1;
					}
					else
					{
						ncorrelativo = 1;
					}
					model.NFactura = ncorrelativo;
					model.ProductosId = 0;
					if (!ModelState.IsValid)
					{
						return View(model);
					}
					var UsuarioId = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
					using (var trans = _context.Database.BeginTransaction())
					{
						try
						{
							
							model.Tasa = (decimal.Parse(model.Total.ToString()) / (t == null? 0 : t.Tasa));
							var facturacion = new Facturacion
							{
								ClienteId = model.ClienteId,
								FechaRegistro = DateTime.Now,
								//Iva = model.Iva,
								NFactura = model.NFactura,
								//SubTotal = model.SubTotal,
								Total = model.Total,
								//TotalIva = model.TotalIva,
								UsuarioId = UsuarioId.Id.ToString(),
								User = UsuarioId,
								Tasa= model.Tasa,
								FormaPagoId = model.FormaPagoId,
								MontoCancelar = Convert.ToDecimal(model.MontoCancelar.Replace(".","").Replace(",","."))
							};
							_context.Add(facturacion);
							await _context.SaveChangesAsync();

							foreach (var item in model.DetalleFacturacionViews.Where(e => !e.Eliminado))
							{
								var detalle = new DetalleFacturas
								{
									Cantidad = item.Cantidad,
									UsuarioId = UsuarioId.Id.ToString(),
									User = UsuarioId,
									FechaRegistro = DateTime.Now,
									FacturacionId = facturacion.FacturacionId,
									ProductosId = item.ProductoId,
									PrecioUnitario = item.PrecioUnitario,
									//SubTotal = item.SubTotal,
									////Iva = 16,
									//IvaUnitario = item.IvaUnitario,
									//TotalIva = item.TotalIva,
									Total = item.Total,
									Facturacion = facturacion,
								};
								_context.Add(detalle);
								await _context.SaveChangesAsync();

								var producto = _context.Productos.FirstOrDefault(p => p.Id == item.ProductoId);
								producto.Stock -= item.Cantidad;
								_context.Update(producto);
								await _context.SaveChangesAsync();
							}
							await _context.SaveChangesAsync();

							//if (model.PedidoId > 0)
							//{
							//    var pedido = _context.Pedidos.Find(model.PedidoId);
							//    pedido.StatusDocumentoId = 2;
							//    _context.Pedidos.Update(pedido);
							//    await _context.SaveChangesAsync();
							//}

							trans.Commit();
							return RedirectToAction(nameof(Index));
						}
						catch (Exception ex)
						{
							trans.Rollback();
							return View(model);
						}
					}
			}

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> GetPrecio(int id)
		{
			var p = await _context.Productos.SingleOrDefaultAsync(t => t.Id == id);
			var data = new ProductoViewModel { Codigo = p.Codigo, Descripcion = p.GetDescripcion, Precio = Convert.ToString(p.Precio), Stock = p.Stock, StockMin = p.StockMin, StockMax = p.StockMax };
			var settings = new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() };
			return Json(data);
		}
		// GET: Facturacion/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var facturacion = await _context.Facturacion.FindAsync(id);
			if (facturacion == null)
			{
				return NotFound();
			}
			ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "GetRif", facturacion.ClienteId);
			return View(facturacion);
		}

		// POST: Facturacion/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Facturacion facturacion)
		{
			if (id != facturacion.FacturacionId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(facturacion);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!FacturacionExists(facturacion.FacturacionId))
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
			ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "GetRif", facturacion.ClienteId);
			return View(facturacion);
		}

		// GET: Facturacion/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var facturacion = await _context.Facturacion
				.Include(f => f.Clientes)
				.Include(d => d.DetalleFacturas)
				.FirstOrDefaultAsync(m => m.FacturacionId == id);
			if (facturacion == null)
			{
				return NotFound();
			}

			return View(facturacion);
		}

		// POST: Facturacion/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var facturacion = await _context.Facturacion.FindAsync(id);
			_context.Facturacion.Remove(facturacion);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		//async Task<FacturacionViewModel> GetPedidoViewAsync(int id)
		//{
		//    var pedido = await _context.Pedidos
		//        .Include(p => p.Clientes)
		//        .Include(i => i.User)
		//        .Include(p => p.StatusDocumento)
		//        .FirstOrDefaultAsync(m => m.DocumentoId == id);
		//    List<DetalleFacturacionViewModel> detalles = new List<DetalleFacturacionViewModel>();

		//    foreach (var item in await _context.DetallePedidos.Include(i => i.Productos).Include(i => i.Productos.Marca).Include(i => i.Productos.Modelo).Where(w => w.DocumentoId == pedido.DocumentoId).ToListAsync())
		//    {
		//        var d = new DetalleFacturacionViewModel
		//        {
		//            Cantidad = item.Cantidad,
		//            DetalleFacturaId = item.DetallePedidoId,
		//            Iva = item.Iva,
		//            IvaUnitario = item.IvaUnitario,
		//            PrecioUnitario = item.PrecioUnitario,
		//            ProductoId = item.ProductoId,
		//            Producto = item.Productos.GetDescripcion,
		//            SubTotal = item.SubTotal,
		//            Total = item.Total,
		//            TotalIva = item.TotalIva,
		//        };
		//        detalles.Add(d);
		//    }
		//    var viewModel = new FacturacionViewModel
		//    {
		//        ClienteId = pedido.ClienteId,
		//        DetalleFacturacionViews = detalles,
		//        NFactura = pedido.NDocumento,
		//        TotalIva = pedido.TotalIva,
		//        Total = pedido.Total,
		//        SubTotal = pedido.SubTotal,
		//        Cliente = pedido.Clientes,
		//        FacturacionId = pedido.DocumentoId,
		//        Iva = pedido.Iva,
		//        PedidoId = pedido.DocumentoId
		//    };
		//    return await Task.FromResult(viewModel);
		//}

		private bool FacturacionExists(int id)
		{
			return _context.Facturacion.Any(e => e.FacturacionId == id);
		}
		public IActionResult RptFactura(int id)
		{
			var data = _context.Facturacion
				.Include(i => i.DetalleFacturas)
				.Include(i => i.Clientes)
				.FirstOrDefault(f => f.FacturacionId == id);
			foreach (var i in data.DetalleFacturas)
			{
				i.Productos = _context.Productos.Find(i.ProductosId);
			}
			//return View(data);
			return new ViewAsPdf(nameof(RptFactura), data)
			{
				PageHeight = 270,
				PageWidth = 80,
				PageMargins = new Rotativa.AspNetCore.Options.Margins(1, 1, 1, 1)
			};
		}
		public IActionResult CierreDiario()
		{
			DateTime f = DateTime.Now;
			var d = new DateTime(f.Year,f.Month,f.Day,23,59,59);
			var h = new DateTime(f.Year, f.Month, f.Day, 0, 0, 0);
			var consulta = _context.Facturacion.Include(d=> d.DetalleFacturas)
				.Include("DetalleFacturas.Productos")
				.Include(i=> i.FormaPago)
				.Include(i=> i.Clientes).Where(f => f.FechaRegistro >= h && f.FechaRegistro <= d).ToList();
			
			return new ViewAsPdf(nameof(CierreDiario), consulta)
			{
				PageMargins = new Rotativa.AspNetCore.Options.Margins(10, 5, 10, 5)
			};
		}
	}
}
