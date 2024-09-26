using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class UserController : Controller
{
    private readonly PM_Context _context;

    public UserController(PM_Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _context.Users.ToListAsync();
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (ModelState.IsValid)
        {
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.Error = "Silme işlemi başarısız oldu.";
            }
        }

        return View(user);
    }
}