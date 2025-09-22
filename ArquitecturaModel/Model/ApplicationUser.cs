
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ArquitecturaModel.Model
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        [Display(Name = "Nombre  ")]
        public string Nombre { get; set; }

        //[Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Apellido ")]
        public string Apellido { get; set; }

        //[Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Direccion  ")]
        public string Direccion { get; set; }

        //[Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Telefono  ")]
        public string Telefono { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string ModifyByUserId { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyDescription { get; set; }
        public string FullName { get => $"{Nombre} {Apellido}"; }
		public int? SucursalesId { get; set; }
		public Sucursales? Sucursales { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
