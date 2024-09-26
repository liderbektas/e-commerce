using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class QuestionsController : Controller
{
    private readonly PM_Context _context;

    public QuestionsController(PM_Context context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> Index()
    {
        var questions = await _context.Questions.ToListAsync();
        if (questions == null)
        {
            return NotFound();
        }
        
        return View(questions);
    }
}