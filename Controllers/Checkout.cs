using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Controllers;

public class Checkout : Controller
{
    private readonly PM_Context _context;

    public Checkout(PM_Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        var user = _context.Users.Find(int.Parse(userId));

        var cart = await _context.Carts
            .Include(x => x.CartItems)
            .ThenInclude(o => o.Products)
            .FirstOrDefaultAsync(x => x.UserId == int.Parse(userId));

        if (cart == null || !cart.CartItems.Any())
        {
            return BadRequest("Sepetiniz Boş");
        }

        var orderInfo = new OrderInfo()
        {
            User = user,
            Cart = cart
        };

        return View(orderInfo);
    }

    [HttpPost]
    public async Task<IActionResult> Index(string address, string paymentMethod, string cardNumber,
        string cardName, string lastDate, string cvv)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(cardNumber))
                {
                    ModelState.AddModelError("", "Adres ve kart numarası boş olamaz.");
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                var cart = await _context.Carts
                    .Include(x => x.CartItems)
                    .ThenInclude(o => o.Products)
                    .FirstOrDefaultAsync(x => x.UserId == int.Parse(userId));

                if (cart == null || !cart.CartItems.Any())
                {
                    return BadRequest("Sepetiniz Boş");
                }


                var order = new Order
                {
                    UserId = int.Parse(userId),
                    OrderDate = DateTime.Now,
                    address = address,
                    PaymentMethod = paymentMethod,
                    CardNumber = cardNumber,
                    CardName = cardName,
                    LastDate = lastDate,
                    CVV = cvv,
                    OrderItems = cart.CartItems.Select(x => new OrderItem
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Products = x.Products
                    }).ToList()
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                cart.CartItems.Clear();
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Success");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Sipariş Alınamadı: " + e.Message);
            }
        }

        return View("Error");
    }
}