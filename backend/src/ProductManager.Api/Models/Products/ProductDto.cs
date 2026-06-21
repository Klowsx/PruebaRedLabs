namespace ProductManager.Api.Models.Products;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public bool Estado { get; set; }
    public string UsuarioCreacion { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public string UsuarioModificacion { get; set; } = string.Empty;
    public DateTime FechaModificacion { get; set; }
}