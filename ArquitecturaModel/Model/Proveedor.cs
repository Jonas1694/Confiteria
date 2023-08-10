using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.Model
{
    public class Proveedor
    {
        public int ProveedorId { get; set; }
		public string TipoDocumento { get; set; }
		[Required(ErrorMessage = "Este campo es obligatorio!")]
		[Display(Name ="Codigo de Proveedor")]
		public string Codigo { get; set; }
		
		[Required(ErrorMessage = "Este campo es obligatorio!")]
		[Display(Name = "Rif o Cedula")]
		[StringLength(9, MinimumLength = 7, ErrorMessage = "El campo {0} debe ser una cadena con una longitud mínima de {2} y una longitud máxima de {1}.")]
		public string Rif { get; set; }
		[Required(ErrorMessage = "Este campo es obligatorio!")]
		public string RazonSocial { get; set; }
		[Required(ErrorMessage = "Este campo es obligatorio!")]
		public string DireccionFiscal { get; set; }
		[Required(ErrorMessage = "Este campo es obligatorio!")]
		public string Telefono { get; set; }
		[Required(ErrorMessage = "Este campo es obligatorio!")]
		public string Correo { get; set; }
		 public string GetRif { get => $"{TipoDocumento}-{Rif} {RazonSocial}"; }
    }
}
