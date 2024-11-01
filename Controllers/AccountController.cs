using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly PM_Context _context;

        public AccountController(PM_Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return NotFound();
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == int.Parse(userId));
            if (user == null)
            {
                return NotFound();
            }

            var orders = _context.Orders
                .Where(o => o.UserId == user.Id)
                .Include(o => o.OrderItems)
                .ThenInclude(p => p.Products)
                .ToList();

            var reviews = _context.Reviews
                .Include(r => r.Products)
                .Where(r => r.UserId == user.Id)
                .ToList();

            var userInfo = new UserViewModel()
            {
                User = user,
                Orders = orders,
                Reviews = reviews
            };

            return View(userInfo);
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
                    if (order == null)
                    {
                        return NotFound();
                    }

                    order.Status = OrderStatus.IptalEdildi;

                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
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