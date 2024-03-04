using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.Model
{
	public class Tenant
	{
        public int TenantID { get; set; }
		public string TipoDocumento { get; set; }
		[Required(ErrorMessage = "Este campo es obligatorio!")]
		[Display(Name = "Rif ")]
		[StringLength(9, MinimumLength = 7, ErrorMessage = "El campo {0} debe ser una cadena con una longitud mínima de {2} y una longitud máxima de {1}.")]
		public string Rif { get; set; }
		public string RazonSocial { get; set; }
		public string DireccionFiscal { get; set; }
		public string Telefono { get; set; }
		public string? Correo { get; set; }
		public DateTime Fecha { get; set; }
	}
}
