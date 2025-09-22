using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.Model
{
    public class DetalleDevolucion
    {
        public int DetalleDevolucionId { get; set; }
        public int DocumentoId { get; set; }
        public Devolucion Devolucion { get; set; }
        public int ProductoId { get; set; }
        public Productos Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal IvaUnitario { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalIva { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int SucursalesId { get; set; }
        public Sucursales Sucursales { get; set; }
    }
}
