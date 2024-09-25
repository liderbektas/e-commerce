namespace ProductManagament_MVC.Models;

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CartItem> CartItems { get; set; } = new();
}

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public bool IsOrdered { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public Products Products { get; set; }
}