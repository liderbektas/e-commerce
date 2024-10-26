using Microsoft.AspNetCore.Mvc;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class ContactController : Controller
{

    private readonly PM_Context _context;

    public ContactController(PM_Context context)
    {
        _context = context;
    }
   
    public IActionResult Index()
    {
        ViewData["ActivePage"] = "Contact";
        var contacts = _context.Contacts.ToList();
        if (contacts == null)
        {
            return NotFound();
        }
        
        return View(contacts);
    }

    public IActionResult Delete(int id)
    {
        var contact = _context.Contacts.FirstOrDefault(c => c.Id == id);
        if (contact == null)
        {
            return NotFound();
        }
        
        return View(contact);
    }

    [HttpPost , ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        if (ModelState.IsValid)
        {
            try
            {   
                var contact = _context.Contacts.FirstOrDefault(c => c.Id == id);
                if (contact == null)
                {
                    return NotFound();
                }

                _context.Contacts.Remove(contact);
                _context.SaveChanges();
                return RedirectToAction("Index");
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