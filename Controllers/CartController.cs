using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Controllers;

public class CartController : Controller
{
    private readonly PM_Context _context;

    public CartController(PM_Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(p => p.Products)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.Now,
                CartItems = new List<CartItem>()
            };

            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var cart = await _context.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.Now,
                CartItems = new List<CartItem>()
            };

            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Ürün bulunamadı.");
            }

            cart.CartItems.Add(new CartItem()
            {
                ProductId = productId,
                Products = product,
                Quantity = quantity
            });
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Products");
    }
}