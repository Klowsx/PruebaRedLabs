using ProductManager.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ProductManager.Infrastructure.Reports;

public class ProductReportDocument : IDocument
{
    private readonly IEnumerable<Product> _productos;

    public ProductReportDocument(IEnumerable<Product> productos)
    {
        _productos = productos;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text(text =>
                {
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
    }

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text("Reporte de Productos")
                    .FontSize(20)
                    .Bold()
                    .FontColor(Colors.Blue.Medium);

                column.Item().Text($"Fecha: {DateTime.Now:yyyy-MM-dd HH:mm}")
                    .FontSize(12)
                    .FontColor(Colors.Grey.Medium);
            });
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.PaddingVertical(20).Column(column =>
        {
            column.Item().Element(ComposeTable);

            var totalProductos = _productos.Count();
            var totalPrecio = _productos.Sum(p => p.Precio);

            column.Item().PaddingTop(20).Text(text =>
            {
                text.Span($"Total de productos: ").Bold();
                text.Span($"{totalProductos}");
            });

            column.Item().Text(text =>
            {
                text.Span($"Suma de precios: ").Bold();
                text.Span($"{totalPrecio:C}");
            });
        });
    }

    private void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(200);
                columns.RelativeColumn();
                columns.ConstantColumn(100);
                columns.ConstantColumn(80);
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("Nombre").Bold();
                header.Cell().Element(CellStyle).Text("Descripción").Bold();
                header.Cell().Element(CellStyle).Text("Precio").Bold();
                header.Cell().Element(CellStyle).Text("Estado").Bold();

                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.FontSize(10))
                        .Padding(5)
                        .Border(1)
                        .BorderColor(Colors.Grey.Lighten1)
                        .Background(Colors.Grey.Lighten3);
                }
            });

            foreach (var producto in _productos)
            {
                table.Cell().Element(CellStyle).Text(producto.Nombre);
                table.Cell().Element(CellStyle).Text(producto.Descripcion ?? "-");
                table.Cell().Element(CellStyle).Text($"{producto.Precio:C}");
                table.Cell().Element(CellStyle).Text(producto.Estado ? "Activo" : "Inactivo");

                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.FontSize(10))
                        .Padding(5)
                        .Border(1)
                        .BorderColor(Colors.Grey.Lighten1);
                }
            }
        });
    }
}