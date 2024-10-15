using Microsoft.AspNetCore.Mvc;
using ProductManagament_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductManagament_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly PM_Context _context;

        public OrderController(PM_Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Order";
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(O => O.OrderItems)
                .ThenInclude(o => o.Products)
                .ToListAsync();
            
            return View(orders);
        }

        public async Task<IActionResult> Details(int id, int productId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Products)
                .ThenInclude(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}