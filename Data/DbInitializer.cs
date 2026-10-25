using Microsoft.EntityFrameworkCore;
using PharmaCareBlazor.Models;

namespace PharmaCareBlazor.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Products.AnyAsync())
        {
            return; // DB has been seeded
        }

        var today = DateTime.Today;

        var sampleProducts = new List<Product>
        {
            new()
            {
                Code = "MED-001",
                Name = "Paracetamol 500mg",
                GenericName = "Acetaminofén",
                Category = "Analgésico",
                Presentation = "Caja x 100 Tabletas",
                BatchNumber = "LOT-2024-A101",
                ExpirationDate = today.AddMonths(18),
                Stock = 120,
                MinStock = 25,
                PurchasePrice = 1.20m,
                SalePrice = 2.50m,
                Laboratory = "Bayer",
                Location = "Estante A-1"
            },
            new()
            {
                Code = "MED-002",
                Name = "Ibuprofeno 600mg",
                GenericName = "Ibuprofeno",
                Category = "Antiinflamatorio",
                Presentation = "Caja x 30 Tabletas",
                BatchNumber = "LOT-2024-B205",
                ExpirationDate = today.AddMonths(14),
                Stock = 8, // Stock Crítico
                MinStock = 20,
                PurchasePrice = 2.00m,
                SalePrice = 4.20m,
                Laboratory = "Pfizer",
                Location = "Estante A-2"
            },
            new()
            {
                Code = "MED-003",
                Name = "Amoxicilina + Clavulánico 875/125mg",
                GenericName = "Amoxicilina / Ácido Clavulánico",
                Category = "Antibiótico",
                Presentation = "Caja x 14 Comprimidos",
                BatchNumber = "LOT-2024-C309",
                ExpirationDate = today.AddMonths(10),
                Stock = 45,
                MinStock = 15,
                PurchasePrice = 8.50m,
                SalePrice = 15.00m,
                Laboratory = "Genfar",
                Location = "Estante B-1"
            },
            new()
            {
                Code = "MED-004",
                Name = "Omeprazol 20mg",
                GenericName = "Omeprazol",
                Category = "Gastroprotector",
                Presentation = "Caja x 28 Cápsulas",
                BatchNumber = "LOT-2023-D114",
                ExpirationDate = today.AddDays(25), // Próximo a vencer!
                Stock = 5, // Stock Crítico
                MinStock = 15,
                PurchasePrice = 3.00m,
                SalePrice = 6.50m,
                Laboratory = "Bagó",
                Location = "Estante B-2"
            },
            new()
            {
                Code = "MED-005",
                Name = "Azitromicina 500mg",
                GenericName = "Azitromicina Dihidrato",
                Category = "Antibiótico",
                Presentation = "Caja x 3 Tabletas",
                BatchNumber = "LOT-2023-E781",
                ExpirationDate = today.AddDays(15), // Próximo a vencer!
                Stock = 14,
                MinStock = 10,
                PurchasePrice = 4.10m,
                SalePrice = 8.00m,
                Laboratory = "Pfizer",
                Location = "Estante B-3"
            },
            new()
            {
                Code = "MED-006",
                Name = "Loratadina 10mg",
                GenericName = "Loratadina",
                Category = "Antihistamínico",
                Presentation = "Caja x 20 Tabletas",
                BatchNumber = "LOT-2024-F442",
                ExpirationDate = today.AddMonths(22),
                Stock = 85,
                MinStock = 15,
                PurchasePrice = 1.50m,
                SalePrice = 3.80m,
                Laboratory = "Sanofi",
                Location = "Estante C-1"
            },
            new()
            {
                Code = "MED-007",
                Name = "Metformina 850mg",
                GenericName = "Metformina Clorhidrato",
                Category = "Antidiabético",
                Presentation = "Caja x 60 Tabletas",
                BatchNumber = "LOT-2024-G901",
                ExpirationDate = today.AddMonths(16),
                Stock = 92,
                MinStock = 20,
                PurchasePrice = 3.20m,
                SalePrice = 7.00m,
                Laboratory = "Merck",
                Location = "Estante C-2"
            },
            new()
            {
                Code = "MED-008",
                Name = "Losartán Potásico 50mg",
                GenericName = "Losartán Potásico",
                Category = "Antihipertensivo",
                Presentation = "Caja x 30 Tabletas",
                BatchNumber = "LOT-2024-H512",
                ExpirationDate = today.AddMonths(20),
                Stock = 60,
                MinStock = 15,
                PurchasePrice = 2.80m,
                SalePrice = 5.90m,
                Laboratory = "Genfar",
                Location = "Estante C-3"
            },
            new()
            {
                Code = "MED-009",
                Name = "Salbutamol Aerosol 100mcg",
                GenericName = "Salbutamol Sulfato",
                Category = "Respiratorio",
                Presentation = "Inhalador x 200 Dosis",
                BatchNumber = "LOT-2024-I330",
                ExpirationDate = today.AddMonths(8),
                Stock = 6, // Stock Crítico
                MinStock = 12,
                PurchasePrice = 6.00m,
                SalePrice = 11.50m,
                Laboratory = "GlaxoSmithKline",
                Location = "Estante D-1"
            },
            new()
            {
                Code = "MED-010",
                Name = "Cetirizina 10mg",
                GenericName = "Cetirizina Diclorhidrato",
                Category = "Antihistamínico",
                Presentation = "Caja x 10 Tabletas",
                BatchNumber = "LOT-2023-J219",
                ExpirationDate = today.AddDays(42), // Próximo a vencer!
                Stock = 38,
                MinStock = 15,
                PurchasePrice = 1.80m,
                SalePrice = 4.00m,
                Laboratory = "Bagó",
                Location = "Estante D-2"
            },
            new()
            {
                Code = "MED-011",
                Name = "Insulina NPH 100 UI/ml",
                GenericName = "Insulina Humana",
                Category = "Antidiabético",
                Presentation = "Frasco Vial 10ml",
                BatchNumber = "LOT-2024-K008",
                ExpirationDate = today.AddMonths(11),
                Stock = 16,
                MinStock = 10,
                PurchasePrice = 18.00m,
                SalePrice = 32.00m,
                Laboratory = "Novo Nordisk",
                Location = "Refrigerador R-1 (2-8°C)"
            },
            new()
            {
                Code = "MED-012",
                Name = "Vitamina C 1000mg Efervescente",
                GenericName = "Ácido Ascórbico",
                Category = "Suplemento",
                Presentation = "Tubo x 10 Tabletas",
                BatchNumber = "LOT-2024-L612",
                ExpirationDate = today.AddMonths(24),
                Stock = 110,
                MinStock = 20,
                PurchasePrice = 2.50m,
                SalePrice = 5.00m,
                Laboratory = "Bayer",
                Location = "Estante E-1"
            },
            new()
            {
                Code = "MED-013",
                Name = "Dexametasona 4mg/2ml",
                GenericName = "Dexametasona Fosfato",
                Category = "Corticoide",
                Presentation = "Caja x 5 Ampollas",
                BatchNumber = "LOT-2023-M890",
                ExpirationDate = today.AddDays(50), // Próximo a vencer!
                Stock = 4, // Stock Crítico
                MinStock = 15,
                PurchasePrice = 3.50m,
                SalePrice = 7.80m,
                Laboratory = "Bagó",
                Location = "Estante E-2"
            },
            new()
            {
                Code = "MED-014",
                Name = "Enalapril Maleato 10mg",
                GenericName = "Enalapril",
                Category = "Antihipertensivo",
                Presentation = "Caja x 30 Comprimidos",
                BatchNumber = "LOT-2024-N412",
                ExpirationDate = today.AddMonths(15),
                Stock = 70,
                MinStock = 15,
                PurchasePrice = 1.90m,
                SalePrice = 4.30m,
                Laboratory = "Genfar",
                Location = "Estante C-4"
            }
        };

        await context.Products.AddRangeAsync(sampleProducts);
        await context.SaveChangesAsync();
    }
}
