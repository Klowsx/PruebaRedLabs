using ProductManager.Domain.Entities;

namespace ProductManager.Domain.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetFilteredAsync(
        string? nombre,
        bool? estado,
        decimal? minPrecio,
        decimal? maxPrecio);

    Task<(IEnumerable<Product> Items, int TotalCount)> GetFilteredPagedAsync(
        string? nombre,
        bool? estado,
        decimal? minPrecio,
        decimal? maxPrecio,
        int pageNumber,
        int pageSize);

    Task<Product> CreateAsync(Product producto);
    Task<Product> UpdateAsync(Product producto);
    Task DeleteAsync(Product producto);
    Task<bool> ExistsAsync(Guid id);
}