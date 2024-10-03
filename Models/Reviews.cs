namespace ProductManagament_MVC.Models;

public class Reviews
{
    public int Id { get; set; }
    public string Review { get; set; }
    public int ProductId { get; set; }
    public Products? Products { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}