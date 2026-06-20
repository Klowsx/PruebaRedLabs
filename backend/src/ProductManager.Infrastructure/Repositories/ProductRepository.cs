using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;
using ProductManager.Infrastructure.Data;

namespace ProductManager.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .OrderByDescending(p => p.FechaCreacion)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetFilteredAsync(
        string? nombre,
        bool? estado,
        decimal? minPrecio,
        decimal? maxPrecio)
    {
        var query = _context.Products.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
        {
            query = query.Where(prod => prod.Nombre.Contains(nombre));
        }

        if (estado.HasValue)
        {
            query = query.Where(prod => prod.Estado == estado.Value);
        }

        if (minPrecio.HasValue)
        {
            query = query.Where(prod => prod.Precio >= minPrecio.Value);
        }

        if (maxPrecio.HasValue)
        {
            query = query.Where(prod => prod.Precio <= maxPrecio.Value);
        }

        return await query
            .OrderByDescending(prod => prod.FechaCreacion)
            .ToListAsync();
    }

    public async Task<Product> CreateAsync(Product producto)
    {
        _context.Products.Add(producto);
        await _context.SaveChangesAsync();
        return producto;
    }

    public async Task<Product> UpdateAsync(Product producto)
    {
        _context.Products.Update(producto);
        await _context.SaveChangesAsync();
        return producto;
    }

    public async Task DeleteAsync(Product producto)
    {
        _context.Products.Remove(producto);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Products.AnyAsync(prod => prod.Id == id);
    }
}