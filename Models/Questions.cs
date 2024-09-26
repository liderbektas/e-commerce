namespace ProductManagament_MVC.Models;

public class Questions
{
    public int Id { get; set; }
    public string Question { get; set; }
    public int ProductId { get; set; }
    public Products? Products { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public DateTime CreatedAt { get; set; }
}