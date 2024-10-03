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
        ViewData["ActivePage"] = "Questions";
        
        var questions = await _context.Questions.ToListAsync();

        var questionsId = questions.Select(q => q.Id).ToList();
        var answers = await _context.Answers
            .Where(a => questionsId.Contains(a.QuestionId))
            .ToListAsync();

       ViewBag.Answer  = answers;
        if (questions == null)
        {
            return NotFound();
        }
        
        return View(questions);
    }
}