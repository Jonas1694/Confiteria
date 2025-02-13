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

        public int ProductoId { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        [Required(ErrorMessage = "De introducir la descripción de la devolución!")]
        [Display(Name = "Descripción")]
        public string DescripcionDevolucion { get; set; }
        public List<DetalleDevolucionViewModel> DetalleDecomentoViews { get; set; }
    }
}
