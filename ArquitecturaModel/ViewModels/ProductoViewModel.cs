using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.ViewModels
{
    public class ProductoViewModel
    {
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio!")]
        [Display(Name = "Producto")]
        public string Descripcion { get; set; }

        //[Required(ErrorMessage = "Este campo es obligatorio!")]
        //[Display(Name = "Descripción Detallada del Producto")]
        //public string DetalleDescripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio!")]
        [Display(Name = "Cantidad del producto")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "* Solo se permiten números.")]
        public double Stock { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio!")]
        [Display(Name = "Stock Minimo")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "* Solo se permiten números.")]
        public int StockMin { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio!")]
        [Display(Name = "Stock Maximo")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "* Solo se permiten números.")]
        public int StockMax { get; set; }

        [Display(Name ="Precio Bs")]
        public string Precio { get; set; }
        public string PrecioDolar { get; set; }

        public string Codigo { get; set; }

    }
}
