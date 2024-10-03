namespace ProductManagament_MVC.Models;

public class Order
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public string? address { get; set; }
    public string? PaymentMethod { get; set; }
    public string? CardNumber { get; set; }
    public string? CardName { get; set; }
    public string? LastDate { get; set; }
    public string? CVV { get; set; }
    public List<OrderItem>? OrderItems { get; set; } = new();
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int? ProductId { get; set; }
    public int? Quantity { get; set; }
    public Products? Products { get; set; }
}