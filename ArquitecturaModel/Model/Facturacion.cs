using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.Model
{
    public class Facturacion
    {
        public int FacturacionId { get; set; }
        public int ClienteId { get; set; }
        public Cliente Clientes { get; set; }
        [Display(Name = "N Documento")]
        public double NFactura { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalIva { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public string UsuarioId { get; set; }
        public ApplicationUser User { get; set; }

        [Display(Name = "Fecha")]
        public DateTime FechaRegistro { get; set; }
        public virtual List<DetalleFacturas> DetalleFacturas { get; set; }
        public decimal Tasa { get; set; }
        public int? FormaPagoId { get; set; }
        public FormaPago FormaPago { get; set; }
        public decimal? MontoCancelar { get; set; }

    }
}
