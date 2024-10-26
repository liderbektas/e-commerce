using Microsoft.AspNetCore.Mvc;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Controllers;

public class ContactController : Controller
{
    private readonly PM_Context _context;

    public ContactController(PM_Context context)
    {
        _context = context;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string username , string email , string message)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var contact = new Contact()
                {
                    UserName = username,
                    Email = email,
                    Message = message,
                    ReceivedDate = DateTime.Now
                };

                await _context.Contacts.AddAsync(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        return View();
    }
}