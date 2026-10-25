using System.ComponentModel.DataAnnotations;

namespace PharmaCareBlazor.DTOs;

public class ProductCreateUpdateDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El código es obligatorio.")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "El código debe tener entre 3 y 20 caracteres.")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre comercial es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El principio activo es obligatorio.")]
    [StringLength(100, ErrorMessage = "El principio activo no puede superar los 100 caracteres.")]
    public string GenericName { get; set; } = string.Empty;

    [Required(ErrorMessage = "La categoría es obligatoria.")]
    public string Category { get; set; } = "Analgésico";

    [Required(ErrorMessage = "La presentación es obligatoria.")]
    public string Presentation { get; set; } = "Caja x 30 Tabletas";

    [Required(ErrorMessage = "El número de lote es obligatorio.")]
    [StringLength(30, ErrorMessage = "El lote no debe superar 30 caracteres.")]
    public string BatchNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
    public DateTime ExpirationDate { get; set; } = DateTime.Today.AddMonths(12);

    [Range(0, 100000, ErrorMessage = "El stock debe ser 0 o superior.")]
    public int Stock { get; set; } = 10;

    [Range(1, 10000, ErrorMessage = "El stock mínimo debe ser al menos 1.")]
    public int MinStock { get; set; } = 10;

    [Range(0.01, 100000.0, ErrorMessage = "El precio de compra debe ser mayor a 0.")]
    public decimal PurchasePrice { get; set; } = 1.00m;

    [Range(0.01, 100000.0, ErrorMessage = "El precio de venta debe ser mayor a 0.")]
    public decimal SalePrice { get; set; } = 1.50m;

    [Required(ErrorMessage = "El laboratorio es obligatorio.")]
    public string Laboratory { get; set; } = "Bayer";

    public string Location { get; set; } = "Estante A-1";
}

public class ProductDto
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
    public int MinStock { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public string Laboratory { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public bool IsLowStock => Stock <= MinStock;
    public bool IsExpired => ExpirationDate.Date < DateTime.Today;
    public bool IsNearExpiration => !IsExpired && (ExpirationDate.Date - DateTime.Today).TotalDays <= 60;
    public int DaysUntilExpiration => (int)(ExpirationDate.Date - DateTime.Today).TotalDays;
}

public class DashboardMetricsDto
{
    public int TotalProducts { get; set; }
    public int CriticalStockCount { get; set; }
    public int ExpiringSoonCount { get; set; }
    public int ExpiredCount { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public int TotalUnitsInStock { get; set; }
}

public class ReportFilterDto
{
    public string ReportType { get; set; } = "all"; // all, near_expiration, critical_stock
    public string Laboratory { get; set; } = "all";
    public string Category { get; set; } = "all";
    public int NearExpirationDays { get; set; } = 60;
}
