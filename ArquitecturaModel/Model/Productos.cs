using System.ComponentModel.DataAnnotations;

namespace ArquitecturaModel.Model
{
    public class Productos
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio!")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "* Solo se permiten números.")]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio!")]
        public string Descripcion { get; set; }
        public string? Imagen { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio!")]
        //[DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Precio { get; set; }
        public decimal PrecioDolar { get; set; }
        public decimal PrecioCosto { get; set; }
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
        public string GetDescripcion { get => $"{Codigo} - {Descripcion}"; }

        [Required(ErrorMessage = "El campo {0} es obligatorio!")]
        public int MarcasId { get; set; }
        public Marcas Marcas { get; set; }

        public int SucursalId { get; set; }
        public Sucursales Sucursales { get; set; }
    }
}
