using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.Model
{
    public class Productos
    {
        public int id { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Stock { get; set; }
        public string Imagen { get; set; }
        public decimal Precio { get; set; }
        public DateTime Fecha { get; set; }
        public int idProveedor { get; set; }
    }
}
