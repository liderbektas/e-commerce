using Microsoft.AspNetCore.Http.HttpResults;

namespace ProductManagament_MVC.Models;

public class Products
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string? Img { get; set; }
    public int? Stock { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public ICollection<Questions> Questions { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Products()
    {
        Questions = new List<Questions>(); 
    }
}