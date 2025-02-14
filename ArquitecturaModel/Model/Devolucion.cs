using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.Model
{
    public class Devolucion
    {
        public int DevolucionId { get; set; }
        public int ClienteId { get; set; }
        public Cliente Clientes { get; set; }
        [Display(Name = "N Documento")]
        public double NDocumento { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalIva { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int StatusDocumentoId { get; set; }
        [Display(Name = "Status")]
        public StatusDocumentos StatusDocumento { get; set; }
        public string DescripcionDevolucion { get; set; }
        public int? FacturacionId { get; set; }
        public Facturacion Facturacion { get; set; }
        [Display(Name = "Fecha")]
        public DateTime FechaRegistro { get; set; }
    }
}
