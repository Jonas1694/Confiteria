using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.Model
{
    public class AplicationRole : IdentityRole<Guid>
    {
        public string DescripcionRole { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
