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

    public IActionResult Edit(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        
        return View(user);
    }

    [HttpPost]
    public IActionResult Edit(int id, string username, string email, string password, string address,
        string phoneNumber , DateTime birthDate)
    {
        try
        {
            var existUser = _context.Users.FirstOrDefault(u => u.Id == id);
            if (existUser == null)
            {
                return NotFound();
            }


            existUser.userName = username;
            existUser.email = email;
            existUser.password = password;
            existUser.Address = address;
            existUser.phoneNumber = phoneNumber;
            existUser.BirthDate = birthDate;

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