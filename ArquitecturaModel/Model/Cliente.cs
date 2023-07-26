using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.Model
{
    public class Cliente
    {
        public int id { get; set; }
        public string TipoDocumento { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio!")]
        [Display(Name = "Rif o Cedula")]
        [StringLength(9, MinimumLength = 7, ErrorMessage = "El campo {0} debe ser una cadena con una longitud mínima de {2} y una longitud máxima de {1}.")]
        public string Rif { get; set; }
        public string RazonSocial { get; set; }
        public string DireccionFiscal { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public DateTime Fecha { get; set; }
        public string GetRif { get => $"{TipoDocumento}-{Rif} {RazonSocial}"; }

    }
}
