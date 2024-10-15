using Microsoft.AspNetCore.Mvc;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class AdminsController : Controller
{
    private readonly PM_Context _context;

    public AdminsController(PM_Context context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        ViewData["ActivePage"] = "Admins";

        var admins = _context.Users
            .Where(u => u.role == "Admin" || u.role ==  "SuperAdmin")
            .ToList();

        if (admins == null)
        {
            return NotFound();
        }

        return View(admins);
    }

    [HttpPost]
    public IActionResult Index(string role, string userName, string email, string password)
    {
        if (ModelState.IsValid)
        {
            var existUser = _context.Users.FirstOrDefault(u => u.email == email);

            if (existUser != null)
            {
                ViewData["Error"] = "Bu e-posta ile kayıtlı bir kullanıcı zaten mevcut.";
            }
            else
            {
                var newAdmin = new User()
                {
                    userName = userName,
                    email = email,
                    password = password,
                    role = role,
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(newAdmin);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
        }
        
        return View();
    }

}