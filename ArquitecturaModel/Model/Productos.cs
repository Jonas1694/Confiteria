using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.Model
{
    public class Productos
    {
        public int id { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio!")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "* Solo se permiten números.")]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio!")]
        public string Descripcion { get; set; }
        public string? Imagen { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio!")]
        public int Precio { get; set; }
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio!")]
        [Display(Name = "Cantidad del producto")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "* Solo se permiten números.")]
        public int Stock { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio!")]
        //[Display(Name = "Stock Minimo")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "* Solo se permiten números.")]
        public int StockMin { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio!")]
        //[Display(Name = "Stock Maximo")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "* Solo se permiten números.")]
        public int StockMax { get; set; }
        public string GetDescripcion { get => $"{Descripcion}"; }
        

    }
}
