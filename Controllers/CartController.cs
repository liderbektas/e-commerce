using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Controllers;

public class CartController : Controller
{
    private readonly PM_Context _context = new();

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(o => o.Products)
            .FirstOrDefaultAsync(o => o.UserId == int.Parse(userId));

        if (order == null)
        {
            return View(new Order { OrderItems = new List<OrderItem>() });
        }

        if (order.OrderItems == null)
        {
            order.OrderItems = new List<OrderItem>();
        }

        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var order = await _context.Orders
            .Include(x => x.OrderItems)
            .FirstOrDefaultAsync(x => x.UserId == int.Parse(userId));

        if (order == null)
        {
            order = new Order
            {
                UserId = int.Parse(userId),
                OrderDate = DateTime.Now,
                OrderItems = new List<OrderItem>()
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        var orderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == productId);
        if (orderItem != null)
        {
            orderItem.Quantity += quantity;
        }
        else
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Ürün bulunamadı.");
            }

            order.OrderItems.Add(new OrderItem()
            {
                ProductId = productId,
                Products = product,
                Quantity = quantity
            });
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Products");
    }

    public async Task<IActionResult> Checkout()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        var order = await _context.Orders
            .Include(x => x.OrderItems)
            .ThenInclude(oi => oi.Products)
            .FirstOrDefaultAsync(x => x.UserId == int.Parse(userId));

        if (order == null)
        {
            return View(new Order { OrderItems = new List<OrderItem>() });
        }

        if (order.OrderItems == null || !order.OrderItems.Any())
        {
            order.OrderItems = new List<OrderItem>();
        }

        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(string shippingFullName, string shippingStreet, string shippingCity,
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

                var order = await _context.Orders.Include(x => x.OrderItems)
                    .ThenInclude(x => x.Products)
                    .FirstOrDefaultAsync(x => x.UserId == int.Parse(userId));

                if (order == null)
                {
                    return NotFound();
                }

                if (!order.OrderItems.Any())
                {
                    return BadRequest("Sepetiniz Boş");
                }


                order.ShippingFullName = shippingFullName;
                order.ShippingStreet = shippingStreet;
                order.ShippingCity = shippingCity;
                order.ShippingState = shippingState;
                order.ShippingZipCode = shippingZipCode;
                order.ShippingCountry = shippingCountry;
                order.PaymentMethod = paymentMethod;
                order.CardNumber = cardNumber;

                await _context.SaveChangesAsync();

                order.OrderItems.Clear();
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Sipariş Alınamadı.");
            }
        }

        return View("Error");
    }
}