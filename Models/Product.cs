namespace PharmaCareBlazor.Models;

public class Product
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string GenericName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Presentation { get; set; } = string.Empty;
    public string BatchNumber { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }
    public int Stock { get; set; }
    public int MinStock { get; set; } = 10;
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public string Laboratory { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Helper calculated properties
    public bool IsLowStock => Stock <= MinStock;
    public bool IsExpired => ExpirationDate.Date < DateTime.Today;
    public bool IsNearExpiration => !IsExpired && (ExpirationDate.Date - DateTime.Today).TotalDays <= 60;
    public int DaysUntilExpiration => (int)(ExpirationDate.Date - DateTime.Today).TotalDays;
}
