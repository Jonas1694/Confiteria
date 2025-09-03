using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.Model
{
    public class Sucursales
    {
        public int SucursalId { get; set; }
		public string TipoDocumento { get; set; }
		[Required(ErrorMessage = "Este campo es obligatorio!")]
		[Display(Name = "Rif ")]
		[StringLength(9, MinimumLength = 7, ErrorMessage = "El campo {0} debe ser una cadena con una longitud mínima de {2} y una longitud máxima de {1}.")]
		public string Rif { get; set; }
		[Display(Name = "Razon Social ")]
		public string SucursalName { get; set; }
		[Display(Name = "Direccion Fiscal ")]
		public string DireccionFiscal { get; set; }
		public DateTime Fecha { get; set; }


	}
}
