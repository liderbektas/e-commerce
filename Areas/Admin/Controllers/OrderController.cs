using Microsoft.AspNetCore.Mvc;
using ProductManagament_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductManagament_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly PM_Context _context = new();

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders.ToListAsync();
            if (orders == null || !orders.Any())
            {
                return NotFound();
            }

            return View(orders);
        }

        public async Task<IActionResult> Details(int id , int productId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Products)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}