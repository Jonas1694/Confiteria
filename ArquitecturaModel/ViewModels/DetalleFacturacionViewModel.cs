using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.ViewModels
{
    public class DetalleFacturacionViewModel
    {
        public int DetalleFacturaId { get; set; }
        public int ProductoId { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal IvaUnitario { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalIva { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public bool Eliminado { get; set; }
		public double Tasa { get; set; }
	}
}
