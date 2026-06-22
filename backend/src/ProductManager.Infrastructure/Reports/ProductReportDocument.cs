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
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Página ").FontSize(9).FontColor(Colors.Grey.Medium);
                    text.CurrentPageNumber().FontSize(9).FontColor(Colors.Grey.Medium);
                    text.Span(" de ").FontSize(9).FontColor(Colors.Grey.Medium);
                    text.TotalPages().FontSize(9).FontColor(Colors.Grey.Medium);
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
                    .FontSize(22)
                    .Bold()
                    .FontColor(Colors.Blue.Darken1);

                column.Item().PaddingTop(4).Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .FontSize(11)
                    .FontColor(Colors.Grey.Medium);
            });
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.PaddingVertical(24).Column(column =>
        {
            column.Spacing(16);
            column.Item().Element(ComposeTable);
            column.Item().Element(ComposeSummary);
        });
    }

    private void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(160);
                columns.RelativeColumn();
                columns.ConstantColumn(90);
                columns.ConstantColumn(90);
            });

            table.Header(header =>
            {
                header.Cell().Element(HeaderCellStyle).Text("Nombre").Bold();
                header.Cell().Element(HeaderCellStyle).Text("Descripción").Bold();
                header.Cell().Element(HeaderCellStyle).AlignRight().Text("Precio").Bold();
                header.Cell().Element(HeaderCellStyle).AlignCenter().Text("Estado").Bold();
            });

            var productList = _productos.ToList();
            for (var i = 0; i < productList.Count; i++)
            {
                var producto = productList[i];
                var isEven = i % 2 == 0;

                table.Cell().Element(cell => RowCellStyle(cell, isEven)).Text(producto.Nombre);
                table.Cell().Element(cell => RowCellStyle(cell, isEven)).Text(producto.Descripcion ?? "—");
                table.Cell().Element(cell => RowCellStyle(cell, isEven)).AlignRight().Text($"{producto.Precio:C}");
                table.Cell().Element(cell => RowCellStyle(cell, isEven)).AlignCenter().Text(producto.Estado ? "Activo" : "Inactivo");
            }
        });
    }

    private void ComposeSummary(IContainer container)
    {
        var totalProductos = _productos.Count();
        var totalPrecio = _productos.Sum(p => p.Precio);

        container.BorderTop(1).BorderColor(Colors.Grey.Lighten2).PaddingTop(12).Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text(text =>
                {
                    text.Span("Total de productos: ").FontSize(11).FontColor(Colors.Grey.Darken1);
                    text.Span(totalProductos.ToString()).FontSize(11).Bold();
                });

                column.Item().PaddingTop(4).Text(text =>
                {
                    text.Span("Suma de precios: ").FontSize(11).FontColor(Colors.Grey.Darken1);
                    text.Span($"{totalPrecio:C}").FontSize(11).Bold();
                });
            });
        });
    }

    private static IContainer HeaderCellStyle(IContainer container)
    {
        return container
            .Background(Colors.Blue.Darken1)
            .BorderBottom(2)
            .BorderColor(Colors.Blue.Darken2)
            .Padding(8)
            .DefaultTextStyle(x => x.FontSize(10).Bold().FontColor(Colors.White));
    }

    private static IContainer RowCellStyle(IContainer container, bool isEven)
    {
        var background = isEven ? Colors.White : Colors.Grey.Lighten4;

        return container
            .Background(background)
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(8)
            .DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Darken2));
    }
}
