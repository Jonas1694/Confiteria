using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.Model
{
    public class Inventario
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        public int ProductosId { get; set; }
        public int SucursalId { get; set; }
        public Productos Productos { get; set; }
        public Sucursales Sucursales { get; set; }
    }
}
