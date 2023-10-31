using ArquitecturaModel.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArquitecturaModel
{
    public class AplicationDbContext : IdentityDbContext<ApplicationUser, AplicationRole, Guid>
    {
        public DbSet<ApplicationUser> Usuarios { get; set; }
        public DbSet<AplicationRole> UserRole { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Proveedor> Proveedors { get; set; }
        public DbSet<Facturacion> Facturacion { get; set; }
        public DbSet<DetalleFacturas> DetalleFacturas { get; set; }
        public DbSet<TipoDocumento> TipoDocumentos { get; set; }
        public DbSet<TasaDolar> TasaDolars { get; set; }
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options)
          : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Cliente>().HasIndex(o => o.RazonSocial).IsUnique();
            base.OnModelCreating(builder);
        }
    }
}