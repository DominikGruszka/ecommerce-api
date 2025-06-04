namespace EcommerceApi.Models;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; } // ← DODAJ TO

    public decimal Price { get; set; }

    public List<Order> Orders { get; set; } = new();
}
