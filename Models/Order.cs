namespace EcommerceApi.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
