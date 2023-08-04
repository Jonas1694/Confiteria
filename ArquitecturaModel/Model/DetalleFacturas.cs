using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.Model
{
    public class DetalleFacturas
    {
        public int DetalleFacturasId { get; set; }
        public int FacturacionId { get; set; }
        public Facturacion Facturacion { get; set; }
        public int ProductosId { get; set; }
        public Productos Productos { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal IvaUnitario { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalIva { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public string UsuarioId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
