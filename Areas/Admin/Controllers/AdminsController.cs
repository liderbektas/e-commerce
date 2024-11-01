using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
                return View(_context.Users.Where(u => u.role == "Admin" || u.role == "Manager").ToList());
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

        ViewData["Error"] = "Formu eksiksiz doldurunuz.";
        return View(_context.Users.Where(u => u.role == "Admin" || u.role == "Manager").ToList());
    }


    [HttpPost]
    public IActionResult Edit(int id, string Username, string Email, string Password, string Address,
        string PhoneNumber, DateTime BirthDate, string userRole)
    {
        try
        {
            var existUser = _context.Users.FirstOrDefault(u => u.Id == id);
            var existEmail = _context.Users.FirstOrDefault(u => u.email == existUser.email && u.Id != id);
            if (existEmail != null)
            {
                TempData["Error3"] = "Bu e-posta ile kayıtlı bir kullanıcı zaten mevcut.";
                return View("Index", _context.Users.Where(u => u.role == "Admin" || u.role == "Manager").ToList());
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
            ViewData["Error"] = "";
            return View("Index", _context.Users.Where(u => u.role == "Admin" || u.role == "Manager").ToList());
        }
    }
}