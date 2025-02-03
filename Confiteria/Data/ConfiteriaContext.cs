using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ArquitecturaModel.Model;

namespace Confiteria.Data
{
    public class ConfiteriaContext : DbContext
    {
        public ConfiteriaContext (DbContextOptions<ConfiteriaContext> options)
            : base(options)
        {
        }

        public DbSet<ArquitecturaModel.Model.Marcas> Marcas { get; set; } = default!;
    }
}
