namespace ProductManagament_MVC.Models;

public class Order
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public string address { get; set; } 
    public string PaymentMethod { get; set; }
    public string CardNumber { get; set; } 
    public string CardName { get; set; } 
    public string LastDate { get; set; }
    public string CVV { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<OrderItem> OrderItems { get; set; } = new();
    public OrderStatus Status { get; set; } = OrderStatus.Hazırlanıyor;
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
    public Products Products { get; set; }
}