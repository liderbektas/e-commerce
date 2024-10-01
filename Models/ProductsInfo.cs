namespace ProductManagament_MVC.Models;

public class ProductsInfo
{
    public List<Products>? Products  { get; set; }
    public List<Category> Category { get; set; }
    public string CategoryName { get; set; }
}