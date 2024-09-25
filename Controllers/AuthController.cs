using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Controllers;

public class AuthController : Controller
{
    private readonly PM_Context _context = new();

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ViewBag.Error = "E-posta ve şifre girilmelidir";
            return View();
        }

        var user = _context.Users.FirstOrDefault(x => x.email == email);

        if (user == null || password != user.password)
        {
            ViewBag.Error = "Geçersiz e-posta veya şifre.";
            return View();
        }

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.userName),
            new Claim(ClaimTypes.Email, user.email),
            new Claim(ClaimTypes.Role, user.role),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principle = new ClaimsPrincipal(identity);

        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Auth");
    }


    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        try
        {
            var existingUser = _context.Users.FirstOrDefault(x => x.email == user.email);

            if (existingUser != null)
            {
                ViewBag.Error = "Bu e-posta adresi ile bir kullanıcı zaten kayıtlı.";
                return View(user);
            }

            user.role = "Müşteri";

            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login", "Auth");
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Bir hata oluştu: {ex.Message}";
            return View(user);
        }
    }
}