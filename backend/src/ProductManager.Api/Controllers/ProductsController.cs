using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Api.Models.Products;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;
using ProductManager.Infrastructure.Reports;
using QuestPDF.Fluent;

namespace ProductManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? nombre,
        [FromQuery] bool? estado,
        [FromQuery] decimal? minPrecio,
        [FromQuery] decimal? maxPrecio,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var productos = await _repository.GetFilteredAsync(nombre, estado, minPrecio, maxPrecio);

        var total = productos.Count();
        var paginados = productos
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = paginados.Select(MapToDto);

        return Ok(new
        {
            Data = dtos,
            Total = total,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(total / (double)pageSize)
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var producto = await _repository.GetByIdAsync(id);

        if (producto == null)
        {
            return NotFound();
        }

        return Ok(MapToDto(producto));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        var userId = User.FindFirst("sub")?.Value ?? "Sistema";

        var producto = new Product
        {
            Id = Guid.NewGuid(),
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            Precio = request.Precio,
            Estado = request.Estado,
            UsuarioCreacion = userId,
            FechaCreacion = DateTime.UtcNow,
            UsuarioModificacion = userId,
            FechaModificacion = DateTime.UtcNow
        };

        var creado = await _repository.CreateAsync(producto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = creado.Id },
            MapToDto(creado));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateProductRequest request)
    {
        var producto = await _repository.GetByIdAsync(id);

        if (producto == null)
        {
            return NotFound();
        }

        var userId = User.FindFirst("sub")?.Value ?? "Sistema";

        producto.Nombre = request.Nombre;
        producto.Descripcion = request.Descripcion;
        producto.Precio = request.Precio;
        producto.Estado = request.Estado;
        producto.UsuarioModificacion = userId;
        producto.FechaModificacion = DateTime.UtcNow;

        var actualizado = await _repository.UpdateAsync(producto);

        return Ok(MapToDto(actualizado));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var producto = await _repository.GetByIdAsync(id);

        if (producto == null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(producto);

        return NoContent();
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetReport(
        [FromQuery] string? nombre,
        [FromQuery] bool? estado,
        [FromQuery] decimal? minPrecio,
        [FromQuery] decimal? maxPrecio)
    {
        var productos = await _repository.GetFilteredAsync(nombre, estado, minPrecio, maxPrecio);

        var document = new ProductReportDocument(productos);
        var pdf = document.GeneratePdf();

        return File(pdf, "application/pdf", $"reporte-productos-{DateTime.Now:yyyyMMddHHmmss}.pdf");
    }

    private static ProductDto MapToDto(Product producto)
    {
        return new ProductDto
        {
            Id = producto.Id,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio,
            Estado = producto.Estado,
            UsuarioCreacion = producto.UsuarioCreacion,
            FechaCreacion = producto.FechaCreacion,
            UsuarioModificacion = producto.UsuarioModificacion,
            FechaModificacion = producto.FechaModificacion
        };
    }
}