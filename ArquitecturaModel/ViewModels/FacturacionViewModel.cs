using ArquitecturaModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.ViewModels
{
	public class FacturacionViewModel
	{
		public int FacturacionId { get; set; }
		[Display(Name = "Cliente")]
		public int ClienteId { get; set; }
		public Cliente? Cliente { get; set; }
		[Display(Name = "N° Factura")]
		public double NFactura { get; set; }
		[Display(Name = "Sub-Total")]
		public decimal SubTotal { get; set; }
		[Display(Name = "Total I.V.A.")]
		public decimal TotalIva { get; set; }
		[Display(Name = "Iva Preferencial")]
		public decimal Iva { get; set; }
		public decimal Total { get; set; }

		[Display(Name = "Producto")]
		public int? ProductosId { get; set; }

		[Display(Name = "Producto")]
		public string Producto { get; set; }
		public int Cantidad { get; set; }
		[Display(Name = "Precio")]
		public decimal PrecioUnitario { get; set; }
		public int StockMin { get; set; }
		public int StockMax { get; set; }
		public int Stock { get; set; }
		public int PedidoId { get; set; }
		public decimal Tasa { get; set; }
		//public string Codigo { get; set; }

		public List<DetalleFacturacionViewModel> DetalleFacturacionViews { get; set; }

		#region
		public FacturacionViewModel AddItems(FacturacionViewModel model)
		{
			List<DetalleFacturacionViewModel> listDetalleCotizacion = new List<DetalleFacturacionViewModel>();

			if (model.DetalleFacturacionViews != null)
				listDetalleCotizacion = model.DetalleFacturacionViews;

			decimal SubTotal = model.PrecioUnitario * model.Cantidad;
			//decimal IvaUnitario = model.PrecioUnitario * (Convert.ToDecimal(FormatPorCentaje(0.16M)) / 100);
			//decimal TotalIva = IvaUnitario * model.Cantidad;
			decimal REF = model.Total/model.Tasa;
			decimal Total = SubTotal /*+ TotalIva*/;
			if (listDetalleCotizacion.Any(a => a.ProductoId == model.ProductosId))
			{
				var i = listDetalleCotizacion.FirstOrDefault(f => f.ProductoId == model.ProductosId);
				i.Cantidad = model.Cantidad + i.Cantidad;
				i.SubTotal = SubTotal;
				//i.Iva = 16;
				//i.IvaUnitario = IvaUnitario;
				//i.TotalIva = TotalIva;
				i.Total = Total;
				i.Tasa = REF;
			}
			else
			{
				listDetalleCotizacion.Add(new DetalleFacturacionViewModel
				{
					ProductoId = model.ProductosId.Value,
					Producto = model.Producto,
					Cantidad = model.Cantidad,
					PrecioUnitario = model.PrecioUnitario,
					SubTotal = SubTotal,
					//Iva = 16,
					//IvaUnitario = IvaUnitario,
					//TotalIva = TotalIva,
					Total = Total,
					Tasa= REF
				});
			}

			model.SubTotal = listDetalleCotizacion.Where(w => w.Eliminado == false).Sum(s => s.SubTotal);
			//model.TotalIva = model.SubTotal * (Convert.ToDecimal(FormatPorCentaje(0.16M)) / 100);
			model.Total = model.SubTotal /*+ model.TotalIva*/;
			model.Tasa = model.Total / Tasa;
			model.DetalleFacturacionViews = listDetalleCotizacion;
			//model.Codigo = "";
			return model;
		}
		public string FormatPorCentaje(decimal valor)
		{
			return valor.ToString("P2").Replace("%", "");
		}
		public FacturacionViewModel ReturnViewModel(Facturacion facturacion, List<DetalleFacturas> detalle)
		{
			List<DetalleFacturacionViewModel> listFac = new List<DetalleFacturacionViewModel>();
			foreach (var item in detalle)
			{
				DetalleFacturacionViewModel d = new DetalleFacturacionViewModel()
				{
					DetalleFacturaId = item.DetalleFacturasId,
					//TotalIva = item.TotalIva,
					Cantidad = item.Cantidad,
					//Iva = item.Iva,
					//IvaUnitario = item.IvaUnitario,
					PrecioUnitario = item.PrecioUnitario,
					Producto = item.Productos.Descripcion,
					ProductoId = item.ProductosId,
					SubTotal = item.SubTotal,
					Total = item.Total,
					Tasa = item.Facturacion.Tasa
					
				};
				listFac.Add(d);
			}

			var view = new FacturacionViewModel
			{
				ClienteId = facturacion.ClienteId,
				Cliente = facturacion.Clientes,
				FacturacionId = facturacion.FacturacionId,
				NFactura = facturacion.NFactura,
				SubTotal = facturacion.SubTotal,
				Total = facturacion.Total,
				Tasa = facturacion.Tasa,
				//TotalIva = facturacion.TotalIva,
				DetalleFacturacionViews = listFac
			};
			return view;
		}
		public FacturacionViewModel ToModel(FacturacionViewModel model)
		{
			List<DetalleFacturacionViewModel> Detalle = DetalleFacturacionViews;
			model.SubTotal = Detalle.Sum(s => s.SubTotal);
			//model.Iva = 16;
			//model.TotalIva = model.SubTotal * (Convert.ToDecimal(FormatPorCentaje(0.16M)) / 100);
			model.Total = model.SubTotal;/* + model.TotalIva;*/
			model.Tasa = model.Total / Tasa;
			model.DetalleFacturacionViews = Detalle;
			return model;
		}

		#endregion

	}
}
