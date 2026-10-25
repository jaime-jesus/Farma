using Microsoft.EntityFrameworkCore;
using PharmaCareBlazor.Common;
using PharmaCareBlazor.Data;
using PharmaCareBlazor.DTOs;
using PharmaCareBlazor.Models;
using PharmaCareBlazor.Services.Interfaces;

namespace PharmaCareBlazor.Services.Implementations;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDto>> GetAllAsync(string? search = null, string? category = null, string? laboratory = null)
    {
        var query = _context.Products.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(term) ||
                p.GenericName.ToLower().Contains(term) ||
                p.Code.ToLower().Contains(term) ||
                p.BatchNumber.ToLower().Contains(term) ||
                p.Laboratory.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(category) && category != "all")
        {
            query = query.Where(p => p.Category == category);
        }

        if (!string.IsNullOrWhiteSpace(laboratory) && laboratory != "all")
        {
            query = query.Where(p => p.Laboratory == laboratory);
        }

        var products = await query.OrderBy(p => p.Name).ToListAsync();
        return products.Select(MapToDto).ToList();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return product == null ? null : MapToDto(product);
    }

    public async Task<Result<ProductDto>> CreateAsync(ProductCreateUpdateDto dto)
    {
        // Business Rule: Code must be unique
        var codeExists = await _context.Products.AnyAsync(p => p.Code.ToLower() == dto.Code.Trim().ToLower());
        if (codeExists)
        {
            return Result<ProductDto>.Failure($"Ya existe un producto registrado con el código '{dto.Code}'.");
        }

        // Business Rule: Sale price should not be less than purchase price
        if (dto.SalePrice < dto.PurchasePrice)
        {
            return Result<ProductDto>.Failure("El precio de venta no puede ser menor al precio de compra.");
        }

        var product = new Product
        {
            Code = dto.Code.Trim().ToUpper(),
            Name = dto.Name.Trim(),
            GenericName = dto.GenericName.Trim(),
            Category = dto.Category,
            Presentation = dto.Presentation,
            BatchNumber = dto.BatchNumber.Trim().ToUpper(),
            ExpirationDate = dto.ExpirationDate,
            Stock = dto.Stock,
            MinStock = dto.MinStock,
            PurchasePrice = dto.PurchasePrice,
            SalePrice = dto.SalePrice,
            Laboratory = dto.Laboratory.Trim(),
            Location = dto.Location.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return Result<ProductDto>.Success(MapToDto(product), "Medicamento registrado exitosamente en el inventario.");
    }

    public async Task<Result<ProductDto>> UpdateAsync(int id, ProductCreateUpdateDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return Result<ProductDto>.Failure("El medicamento no fue encontrado en la base de datos.");
        }

        // Business Rule: Code uniqueness for other records
        var codeExists = await _context.Products.AnyAsync(p => p.Id != id && p.Code.ToLower() == dto.Code.Trim().ToLower());
        if (codeExists)
        {
            return Result<ProductDto>.Failure($"El código '{dto.Code}' ya está siendo utilizado por otro producto.");
        }

        if (dto.SalePrice < dto.PurchasePrice)
        {
            return Result<ProductDto>.Failure("El precio de venta no puede ser menor al precio de compra.");
        }

        product.Code = dto.Code.Trim().ToUpper();
        product.Name = dto.Name.Trim();
        product.GenericName = dto.GenericName.Trim();
        product.Category = dto.Category;
        product.Presentation = dto.Presentation;
        product.BatchNumber = dto.BatchNumber.Trim().ToUpper();
        product.ExpirationDate = dto.ExpirationDate;
        product.Stock = dto.Stock;
        product.MinStock = dto.MinStock;
        product.PurchasePrice = dto.PurchasePrice;
        product.SalePrice = dto.SalePrice;
        product.Laboratory = dto.Laboratory.Trim();
        product.Location = dto.Location.Trim();

        await _context.SaveChangesAsync();

        return Result<ProductDto>.Success(MapToDto(product), "Medicamento actualizado correctamente.");
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return Result.Failure("El medicamento no existe o ya fue eliminado.");
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return Result.Success($"El medicamento '{product.Name}' fue eliminado del inventario.");
    }

    public async Task<DashboardMetricsDto> GetMetricsAsync()
    {
        var today = DateTime.Today;
        var limitDate = today.AddDays(60);

        var products = await _context.Products.AsNoTracking().ToListAsync();

        return new DashboardMetricsDto
        {
            TotalProducts = products.Count,
            CriticalStockCount = products.Count(p => p.Stock <= p.MinStock),
            ExpiringSoonCount = products.Count(p => p.ExpirationDate.Date >= today && p.ExpirationDate.Date <= limitDate),
            ExpiredCount = products.Count(p => p.ExpirationDate.Date < today),
            TotalInventoryValue = products.Sum(p => p.Stock * p.SalePrice),
            TotalUnitsInStock = products.Sum(p => p.Stock)
        };
    }

    public async Task<List<ProductDto>> GetExpiringSoonAsync(int days = 60)
    {
        var today = DateTime.Today;
        var limitDate = today.AddDays(days);

        var items = await _context.Products.AsNoTracking()
            .Where(p => p.ExpirationDate.Date <= limitDate)
            .OrderBy(p => p.ExpirationDate)
            .ToListAsync();

        return items.Select(MapToDto).ToList();
    }

    public async Task<List<ProductDto>> GetCriticalStockAsync()
    {
        var items = await _context.Products.AsNoTracking()
            .Where(p => p.Stock <= p.MinStock)
            .OrderBy(p => p.Stock)
            .ToListAsync();

        return items.Select(MapToDto).ToList();
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        return await _context.Products.AsNoTracking()
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }

    public async Task<List<string>> GetLaboratoriesAsync()
    {
        return await _context.Products.AsNoTracking()
            .Select(p => p.Laboratory)
            .Distinct()
            .OrderBy(l => l)
            .ToListAsync();
    }

    private static ProductDto MapToDto(Product p) => new()
    {
        Id = p.Id,
        Code = p.Code,
        Name = p.Name,
        GenericName = p.GenericName,
        Category = p.Category,
        Presentation = p.Presentation,
        BatchNumber = p.BatchNumber,
        ExpirationDate = p.ExpirationDate,
        Stock = p.Stock,
        MinStock = p.MinStock,
        PurchasePrice = p.PurchasePrice,
        SalePrice = p.SalePrice,
        Laboratory = p.Laboratory,
        Location = p.Location
    };
}
