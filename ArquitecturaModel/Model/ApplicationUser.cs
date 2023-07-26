
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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

    }
}
