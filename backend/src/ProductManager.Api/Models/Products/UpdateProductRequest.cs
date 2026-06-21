using System.ComponentModel.DataAnnotations;

namespace ProductManager.Api.Models.Products;

public class UpdateProductRequest
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "La descripción no puede tener más de 1000 caracteres.")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
    public decimal Precio { get; set; }

    public bool Estado { get; set; }
}