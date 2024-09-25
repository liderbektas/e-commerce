namespace ProductManagament_MVC.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public string ShippingFullName { get; set; }
    public string ShippingStreet { get; set; }
    public string ShippingCity { get; set; }
    public string ShippingState { get; set; }
    public string ShippingZipCode { get; set; }
    public string ShippingCountry { get; set; }
    public string PaymentMethod { get; set; }
    public string CardNumber { get; set; }
    public List<OrderItem>? OrderItems { get; set; } = new();
}

public class OrderItem
{
    public int Id { get; set; }
    public int? OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public Products? Products { get; set; }
}