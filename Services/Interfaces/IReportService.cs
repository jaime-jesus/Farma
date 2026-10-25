using PharmaCareBlazor.DTOs;

namespace PharmaCareBlazor.Services.Interfaces;

public class ReportDataResult
{
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; } = DateTime.Now;
    public string GeneratedBy { get; set; } = "Sistema PharmaCare";
    public List<ProductDto> Items { get; set; } = new();
    public int TotalProducts => Items.Count;
    public int TotalUnits => Items.Sum(i => i.Stock);
    public decimal TotalValue => Items.Sum(i => i.Stock * i.SalePrice);
    public decimal TotalCost => Items.Sum(i => i.Stock * i.PurchasePrice);
    public decimal EstimatedMargin => TotalValue - TotalCost;
}

public interface IReportService
{
    Task<ReportDataResult> GenerateReportAsync(ReportFilterDto filter, string currentUserName);
}
