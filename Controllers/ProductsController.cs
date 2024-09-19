using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Controllers;

public class ProductsController : Controller
{
    private readonly PM_Context _context = new();

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.ToListAsync();
        if (products == null)
        {
            return NotFound();
        }

        return View(products);
    }
}