using PharmaCareBlazor.DTOs;
using PharmaCareBlazor.Services.Interfaces;

namespace PharmaCareBlazor.Services.Implementations;

public class ReportService : IReportService
{
    private readonly IProductService _productService;

    public ReportService(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ReportDataResult> GenerateReportAsync(ReportFilterDto filter, string currentUserName)
    {
        var allProducts = await _productService.GetAllAsync(laboratory: filter.Laboratory, category: filter.Category);
        var today = DateTime.Today;

        var result = new ReportDataResult
        {
            GeneratedAt = DateTime.Now,
            GeneratedBy = string.IsNullOrWhiteSpace(currentUserName) ? "Administrador Farmacéutico" : currentUserName
        };

        switch (filter.ReportType)
        {
            case "near_expiration":
                result.Title = "INFORME DE MEDICAMENTOS PRÓXIMOS A VENCER";
                result.Subtitle = $"Control de Lotes y Vencimientos dentro de los próximos {filter.NearExpirationDays} días";
                var limitDate = today.AddDays(filter.NearExpirationDays);
                result.Items = allProducts
                    .Where(p => p.ExpirationDate.Date <= limitDate)
                    .OrderBy(p => p.ExpirationDate)
                    .ToList();
                break;

            case "critical_stock":
                result.Title = "INFORME DE CONTROL DE STOCK CRÍTICO";
                result.Subtitle = "Medicamentos con existencias iguales o inferiores al stock mínimo de reposición";
                result.Items = allProducts
                    .Where(p => p.IsLowStock)
                    .OrderBy(p => p.Stock)
                    .ToList();
                break;

            case "all":
            default:
                result.Title = "REPORTE GENERAL DE INVENTARIO VALORIZADO";
                result.Subtitle = "Listado completo de productos farmacéuticos activos en farmacia";
                result.Items = allProducts.OrderBy(p => p.Category).ThenBy(p => p.Name).ToList();
                break;
        }

        return result;
    }
}
