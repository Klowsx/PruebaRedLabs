namespace ProductManager.Api.Models.Products;

public class ProductFilterRequest
{
    public string? Nombre { get; set; }
    public bool? Estado { get; set; }
    public decimal? MinPrecio { get; set; }
    public decimal? MaxPrecio { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}