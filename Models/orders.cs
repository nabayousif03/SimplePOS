namespace SimplePOS.Models;

public class orders
{
    public Guid Id { get; set; }
    public int totalPrice { get; set; } 
    public string Status { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime DeliveryDate { get; set; }
}