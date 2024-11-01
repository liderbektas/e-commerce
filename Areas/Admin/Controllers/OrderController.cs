using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var statusList = Enum.GetValues(typeof(OrderStatus))
                .Cast<OrderStatus>()
                .Select(s => new SelectListItem()
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString()
                }).ToList();

            ViewBag.Status = new SelectList(statusList, "Value", "Text", order.Status);
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Details(int id , OrderStatus status)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var order = await _context.Orders.FindAsync(id);

                    if (order == null)
                    {
                        return NotFound();
                    }

                    order.Status = status;
                    _context.SaveChanges();
                    return RedirectToAction("Details");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return View();
        }
    }
}