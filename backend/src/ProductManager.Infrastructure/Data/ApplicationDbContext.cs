using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Entities;

namespace ProductManager.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Productos");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.Descripcion)
                .HasMaxLength(1000);

            entity.Property(p => p.Precio)
                .IsRequired()
                .HasPrecision(18, 2);

            entity.Property(p => p.Estado)
                .IsRequired();

            entity.Property(p => p.UsuarioCreacion)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.FechaCreacion)
                .IsRequired();

            entity.Property(p => p.UsuarioModificacion)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.FechaModificacion)
                .IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Usuarios");

            entity.HasKey(u => u.Id);

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(200);
        });
    }
}