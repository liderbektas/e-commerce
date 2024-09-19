namespace ProductManagament_MVC.Models;

public class Products
{
    public int Id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public decimal price { get; set; }
    public string? img { get; set; }
    public int? UserId { get; set; }

    public User? User { get; set; }
}