using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Controllers;

public class AuthController : Controller
{
    private readonly PM_Context _context;

    public AuthController(PM_Context context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        try
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "E-posta ve şifre girilmelidir.";
                return RedirectToAction("Index", "Home");
            }

            var user = _context.Users.FirstOrDefault(x => x.email == email);

            if (user == null || password != user.password)
            {
                TempData["Error"] = "Geçersiz e-posta veya şifre.";
                return RedirectToAction("Index", "Home");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.userName),
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.Role, user.role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            TempData["Error"] = "Bir hata oluştu.";
            return RedirectToAction("Index", "Home");
        }
    }


    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(string userName, string email, string password)
    {
        try
        {
            var existingUser = _context.Users.FirstOrDefault(x => x.email == email);

            if (existingUser != null)
            {
                ViewBag.Error = "Bu e-posta adresi ile bir kullanıcı zaten kayıtlı.";
                return View();
            }

            var user = new User
            {
                userName = userName,
                email = email,
                password = password,
                role = "Müşteri",
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Bir hata oluştu: {ex.Message}";
            return View();
        }
    }

    [HttpPost]
    public IActionResult Edit(int userId, string address, string phone, DateTime birthDate, string email,
        string userName)
    {
        var user = _context.Users.Find(userId);
        if (user == null)
        {
            return NotFound();
        }

        user.phoneNumber = phone;
        user.BirthDate = birthDate;
        user.email = email;
        user.userName = userName;
        user.Address = address;

        _context.Users.Update(user);
        _context.SaveChanges();
        return RedirectToAction("Index", "Account");
    }

    [HttpPost]
    public IActionResult DeleteAddress(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        user.Address = null;

        _context.Users.Update(user);
        _context.SaveChanges();
        return RedirectToAction("Index", "Account");
    }
}