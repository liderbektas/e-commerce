using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            .Where(u => u.role == "Admin" || u.role == "Manager")
            .ToList();

        if (admins == null)
        {
            return NotFound();
        }

        return View(admins);
    }

    [HttpPost]
    public IActionResult Index(string role, string Username, string Email, string Password)
    {
        if (ModelState.IsValid)
        {
            var existUser = _context.Users.FirstOrDefault(u => u.email == Email);

            if (existUser != null)
            {
                ViewData["Error"] = "Bu e-posta ile kayıtlı bir kullanıcı zaten mevcut.";
            }
            else
            {
                var newAdmin = new User()
                {
                    userName = Username,
                    email = Email,
                    password = Password,
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
    
    [HttpPost]
    public IActionResult Edit(int id, string Username, string Email, string Password, string Address,
        string PhoneNumber , DateTime BirthDate , string userRole)
    {
        try
        {
            var existUser = _context.Users.FirstOrDefault(u => u.Id == id);
            if (existUser == null)
            {
                return NotFound();
            }
            
            existUser.userName = Username;
            existUser.email = Email;
            existUser.password = Password;
            existUser.Address = Address;
            existUser.phoneNumber = PhoneNumber;
            existUser.BirthDate = BirthDate;
            existUser.role = userRole;

            _context.Users.Update(existUser);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            ViewBag.Error = e.Message;
        }

        return View();
    }
}