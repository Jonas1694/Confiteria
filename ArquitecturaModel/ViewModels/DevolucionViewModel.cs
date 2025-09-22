using ArquitecturaModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.ViewModels
{
    public class DevolucionViewModel
    {
        public int DocumentoId { get; set; }
        public int FacturaId { get; set; }
        [Required(ErrorMessage = "Debe seleccionar el cliente!")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }
        public Cliente Clientes { get; set; }
        [Display(Name = "N° Factura")]
        public double NDocumento { get; set; }
        [Display(Name = "Sub-Total")]
        public decimal SubTotal { get; set; }
        [Display(Name = "Total I.V.A.")]
        public decimal TotalIva { get; set; }
        [Display(Name = "Iva Preferencial")]
        public decimal Iva { get; set; }
        public decimal Total { get; set; }

		[Display(Name = "Producto")]
		public int ProductoId { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Tasa { get; set; }
        public decimal PrecioUnitario { get; set; }
        [Required(ErrorMessage = "De introducir la descripción de la devolución!")]
        [Display(Name = "Descripción")]
        public string DescripcionDevolucion { get; set; }
        public int SucursalesId { get; set; }
        public List<DetalleDevolucionViewModel> DetalleDocumentoViews { get; set; }
        #region
        public DevolucionViewModel ReturnViewModel(Devolucion compras, List<DetalleDevolucion> detalle)
        {

            List<DetalleDevolucionViewModel> list = new List<DetalleDevolucionViewModel>();
            foreach (var item in detalle)
            {
                list.Add(new DetalleDevolucionViewModel
                {
                    ProductoId = item.ProductoId,
                    Producto = item.Producto.GetDescripcion,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioUnitario,
                    SubTotal = item.SubTotal,
                    Iva = 12,
                    IvaUnitario = item.IvaUnitario,
                    TotalIva = item.TotalIva,
                    Tasa= item.Devolucion.Tasa,
                    Total = item.Total
                });
            }
            var view = new DevolucionViewModel
            {
                DocumentoId = compras.DevolucionId,
                Iva = compras.Iva,
                NDocumento = compras.NDocumento,
                Clientes = compras.Clientes,
                SubTotal = compras.SubTotal,
                Total = compras.Total,
                TotalIva = compras.TotalIva,
                Tasa= compras.Tasa,
                ClienteId = compras.ClienteId,
                DetalleDocumentoViews = list
            };
            return view;
        }
        public DevolucionViewModel AddItems(DevolucionViewModel model)
        {
            List<DetalleDevolucionViewModel> listDetalleCotizacion = new List<DetalleDevolucionViewModel>();

            if (model.DetalleDocumentoViews != null)
                listDetalleCotizacion = model.DetalleDocumentoViews;

            decimal SubTotal = model.PrecioUnitario * model.Cantidad;
            decimal IvaUnitario = model.PrecioUnitario * (Convert.ToDecimal(FormatPorCentaje(0.16M)) / 100);
            decimal TotalIva = IvaUnitario * model.Cantidad;
            decimal Total = SubTotal + TotalIva;
			decimal REF = model.Total / model.Tasa;
			listDetalleCotizacion.Add(new DetalleDevolucionViewModel
            {
                ProductoId = model.ProductoId,
                Producto = model.Producto,
                Cantidad = model.Cantidad,
                PrecioUnitario = model.PrecioUnitario,
                SubTotal = SubTotal,
                Iva = 16,
                IvaUnitario = IvaUnitario,
                TotalIva = TotalIva,
                Tasa= model.Tasa,
                Total = Total
            });

            model.SubTotal = listDetalleCotizacion.Sum(s => s.SubTotal);
            model.TotalIva = model.SubTotal * (Convert.ToDecimal(FormatPorCentaje(0.16M)) / 100);
            model.Total = model.SubTotal + model.TotalIva;
            model.DetalleDocumentoViews = listDetalleCotizacion;
            return model;
        }
        public string FormatPorCentaje(decimal valor)
        {
            return valor.ToString("P2").Replace("%", "");
        }
        public DevolucionViewModel ToModel(DevolucionViewModel model)
        {
            List<DetalleDevolucionViewModel> Detalle = DetalleDocumentoViews;
            model.SubTotal = Detalle.Sum(s => s.SubTotal);
            model.Iva = 16;
            model.TotalIva = model.SubTotal * (Convert.ToDecimal(FormatPorCentaje(0.16M)) / 100);
            model.Total = model.SubTotal + model.TotalIva;
            model.DetalleDocumentoViews = Detalle;
            return model;
        }
        #endregion
    }
}
