using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.Model
{
    public class Proveedor
    {
        public int id { get; set; }
        public string Codigo { get; set; }
        public string Rif { get; set; }
        public string RazonSocial { get; set; }
        public string DireccionFiscal { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
    }
}
