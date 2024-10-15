using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagament_MVC.Models;


namespace ProductManagament_MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class MainController : Controller
{
    private readonly PM_Context _context;

    public MainController(PM_Context context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> Index()
    {
        ViewData["ActivePage"] = "Panel";

        var users = await _context.Users.ToListAsync();
        var products = await _context.Products.ToListAsync();
        var orders = await _context.Orders.ToListAsync();
        var categories = await _context.Categories.ToListAsync();
        
        var lastCustomers = users.OrderByDescending(u => u.CreatedAt)
            .Take(3)
            .ToList();
        
        var lastPrroducts =  products.OrderByDescending(u => u.CreatedAt)
            .Take(3)
            .ToList();
        
        var lastOrders =  orders.OrderByDescending(u => u.CreatedAt)
            .Take(3)
            .ToList();
        
        

        var panelInfos = new PanelViewModel()
        {
            Users = users,
            ProductsList = products,
            Orders = orders,
            LastCustomers = lastCustomers,
            LastOrders = lastOrders,
            LastProducts = lastPrroducts,
        };
    
        return View(panelInfos);
    }

}