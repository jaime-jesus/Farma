using Microsoft.EntityFrameworkCore;
using PharmaCareBlazor.Models;

namespace PharmaCareBlazor.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.GenericName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Presentation).IsRequired().HasMaxLength(50);
            entity.Property(e => e.BatchNumber).IsRequired().HasMaxLength(30);
            entity.Property(e => e.Laboratory).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.SalePrice).HasColumnType("decimal(18,2)");
        });
    }
}
