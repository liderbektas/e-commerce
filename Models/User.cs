using Microsoft.AspNetCore.Identity;

namespace ProductManagament_MVC.Models;

public class User 
{
    public int Id { get; set; }
    public string userName { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string role { get; set; }
    public string? Address { get; set; }
    public string? phoneNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? CreatedAt { get; set; }
}