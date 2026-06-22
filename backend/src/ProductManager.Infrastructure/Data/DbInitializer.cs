using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Entities;

namespace ProductManager.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        await SeedProductsAsync(context);
    }

    private static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        if (await context.Products.AnyAsync())
        {
            return;
        }

        var userId = "Sistema";
        var now = DateTime.UtcNow;

        var products = new List<Product>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Nombre = "Laptop Dell",
                Descripcion = "Laptop ultraligera con procesador Intel Core i7",
                Precio = 1299.99m,
                Estado = true,
                UsuarioCreacion = userId,
                FechaCreacion = now,
                UsuarioModificacion = userId,
                FechaModificacion = now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Nombre = "Monitor LG 27 pulgadas",
                Descripcion = "Monitor 4K IPS para diseño y productividad",
                Precio = 449.99m,
                Estado = true,
                UsuarioCreacion = userId,
                FechaCreacion = now,
                UsuarioModificacion = userId,
                FechaModificacion = now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Nombre = "Teclado mecánico Keychron",
                Descripcion = "Teclado inalámbrico con switches rojos",
                Precio = 89.99m,
                Estado = true,
                UsuarioCreacion = userId,
                FechaCreacion = now,
                UsuarioModificacion = userId,
                FechaModificacion = now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Nombre = "Mouse Logitech MX Master 3S",
                Descripcion = "Mouse ergonómico de alta precisión",
                Precio = 99.99m,
                Estado = false,
                UsuarioCreacion = userId,
                FechaCreacion = now,
                UsuarioModificacion = userId,
                FechaModificacion = now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Nombre = "Auriculares Sony WH-1000XM5",
                Descripcion = "Auriculares con cancelación de ruido",
                Precio = 379.99m,
                Estado = true,
                UsuarioCreacion = userId,
                FechaCreacion = now,
                UsuarioModificacion = userId,
                FechaModificacion = now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Nombre = "Webcam Logitech C920",
                Descripcion = "Cámara web Full HD para videollamadas",
                Precio = 69.99m,
                Estado = true,
                UsuarioCreacion = userId,
                FechaCreacion = now,
                UsuarioModificacion = userId,
                FechaModificacion = now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Nombre = "Disco duro externo 2TB",
                Descripcion = "Almacenamiento portátil USB 3.0",
                Precio = 79.99m,
                Estado = false,
                UsuarioCreacion = userId,
                FechaCreacion = now,
                UsuarioModificacion = userId,
                FechaModificacion = now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Nombre = "Router WiFi 6 TP-Link",
                Descripcion = "Router de doble banda con WiFi 6",
                Precio = 129.99m,
                Estado = true,
                UsuarioCreacion = userId,
                FechaCreacion = now,
                UsuarioModificacion = userId,
                FechaModificacion = now
            }
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }
}
