using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.ViewModels
{
    public class ChangePasswordViewModel
    {
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Nueva Contraseña")]
        public string NewPassword { get; set; }

        [DisplayName("Confirmar Nueva Contraseña")]
        public string ConfirmPassword { get; set; }
    }
}
