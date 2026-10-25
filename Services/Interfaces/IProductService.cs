using PharmaCareBlazor.Common;
using PharmaCareBlazor.DTOs;

namespace PharmaCareBlazor.Services.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync(string? search = null, string? category = null, string? laboratory = null);
    Task<ProductDto?> GetByIdAsync(int id);
    Task<Result<ProductDto>> CreateAsync(ProductCreateUpdateDto dto);
    Task<Result<ProductDto>> UpdateAsync(int id, ProductCreateUpdateDto dto);
    Task<Result> DeleteAsync(int id);
    Task<DashboardMetricsDto> GetMetricsAsync();
    Task<List<ProductDto>> GetExpiringSoonAsync(int days = 60);
    Task<List<ProductDto>> GetCriticalStockAsync();
    Task<List<string>> GetCategoriesAsync();
    Task<List<string>> GetLaboratoriesAsync();
}
