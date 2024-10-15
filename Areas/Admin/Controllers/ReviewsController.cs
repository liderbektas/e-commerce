using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class ReviewsController : Controller
{
    private readonly PM_Context _context;

    public ReviewsController(PM_Context context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        ViewData["ActivePage"] = "Reviews";
        
        var reviews = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Products)
            .ToListAsync();
        if (reviews == null)
        {
            return NotFound();
        }
        
        return View(reviews);
    }
}