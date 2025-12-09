using System.Text.Json.Serialization;

namespace SimplePOS.Models;

public class OrderItem
{
    public int Id { get; set; }
        
    
    public Guid OrderId { get; set; }
    [JsonIgnore] 
    public orders Order { get; set; }

   
    public int MenuItemId { get; set; }
    public Items MenuItem { get; set; }

    public int Quantity { get; set; }
}