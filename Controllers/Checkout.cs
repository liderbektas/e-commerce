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

        var cart = await _context.Carts
            .Include(x => x.CartItems)
            .ThenInclude(o => o.Products)
            .FirstOrDefaultAsync(x => x.UserId == int.Parse(userId));

        if (cart == null || !cart.CartItems.Any())
        {
            return BadRequest("Sepetiniz Boş");
        }

        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> Index(string shippingFullName, string shippingStreet, string shippingCity,
        string shippingState, string shippingZipCode, string shippingCountry, string paymentMethod, string cardNumber)
    {
        if (ModelState.IsValid)
        {
            try
            {
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
                    ShippingFullName = shippingFullName,
                    ShippingStreet = shippingStreet,
                    ShippingCity = shippingCity,
                    ShippingState = shippingState,
                    ShippingZipCode = shippingZipCode,
                    ShippingCountry = shippingCountry,
                    PaymentMethod = paymentMethod,
                    CardNumber = cardNumber,
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