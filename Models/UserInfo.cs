namespace ProductManagament_MVC.Models;

public class UserInfo
{
    public User? User { get; set; }
    public List<Order>? Orders { get; set; }
    public List<Reviews>? Reviews { get; set; }
}