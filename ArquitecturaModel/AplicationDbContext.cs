using ArquitecturaModel.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
        public DbSet<TasaDolar> TasaDolar { get; set; }
        public DbSet<FormaPago> FormaPago { get; set; }
        public DbSet<Marcas> Marcas { get; set; }
        public DbSet<StatusDocumentos> StatusDocumentos { get; set; }
        public DbSet<Devolucion> Devolucions { get; set; }
        public DbSet<DetalleDevolucion> DetalleDevoluciones { get; set; }
        public DbSet<Sucursales> Sucursales { get; set; }
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options)
          : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Cliente>().HasIndex(o => o.RazonSocial).IsUnique();
            builder.Entity<StatusDocumentos>().HasKey(i => i.StatusDocumentoId);
            builder.Entity<DetalleDevolucion>().HasOne(d => d.Devolucion).WithMany().HasForeignKey(d => d.DocumentoId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Sucursales>().HasIndex(s => s.SucursalName).IsUnique();
			builder.Entity<Sucursales>().HasKey(s => s.SucursalId);
			base.OnModelCreating(builder);
        }
    }
}