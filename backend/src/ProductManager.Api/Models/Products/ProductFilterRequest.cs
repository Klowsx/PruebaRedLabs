using System.ComponentModel.DataAnnotations;

namespace ProductManager.Api.Models.Products;

public class ProductFilterRequest
{
    public string? Nombre { get; set; }
    public bool? Estado { get; set; }
    public decimal? MinPrecio { get; set; }
    public decimal? MaxPrecio { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La página debe ser mayor o igual a 1.")]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "El tamaño de página debe estar entre 1 y 100.")]
    public int PageSize { get; set; } = 10;
}