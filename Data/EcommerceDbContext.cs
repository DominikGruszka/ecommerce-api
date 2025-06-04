using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Data;

public class EcommerceDbContext : DbContext
{
    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Orders)
            .WithMany(o => o.Products)
            .UsingEntity(j => j.ToTable("OrderProducts"));
    }
}
