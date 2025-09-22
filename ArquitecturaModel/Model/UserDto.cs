using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.Model
{
    public class UserDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Nombre  ")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Apellido ")]
        public string Apellido { get; set; }

        //[Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Direccion  ")]
        public string Direccion { get; set; }

        //[Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Telefono  ")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} y un máximo de {1} caracteres de longitud.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }

        public int SucursalId { get; set; }
    }
}
