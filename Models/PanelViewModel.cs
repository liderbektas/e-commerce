namespace ProductManagament_MVC.Models;

public class PanelViewModel
{
    public List<User> Users { get; set; }
    public List<Products> ProductsList { get; set; }
    public List<Order> Orders { get; set; }
    public List<User> LastCustomers { get; set; } 
    public List<Products> LastProducts { get; set; } 
    public List<Order> LastOrders  { get; set; } 
    public List<Category> Categories  { get; set; } 
    public List<dynamic> ProductCountsByCategory { get; set; }
}