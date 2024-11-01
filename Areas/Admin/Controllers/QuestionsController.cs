using System.Security.Claims;
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

        var questions = await _context.Questions
            .Include(q => q.Products)
            .Include(q => q.User)
            .ToListAsync();

        var questionIds = questions.Select(q => q.Id).ToList();

        var answers = await _context.Answers
            .Where(a => questionIds.Contains(a.QuestionId))
            .ToListAsync();

        var questionViewModels = questions.Select(q => new QuestionsViewModel()
        {
            Questions = q,
            Answer = answers.FirstOrDefault(a => a.QuestionId == q.Id)
        }).ToList();

        return View(questionViewModels);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(int questionId, string questionContent, string answerContent)
    {
        try
        {
            var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == questionId);
            if (question != null)
            {
                question.Question = questionContent;
                _context.Questions.Update(question);
            }

            var answer = await _context.Answers.FirstOrDefaultAsync(a => a.QuestionId == questionId);
            if (answer != null)
            {
                answer.Content = answerContent;
                _context.Answers.Update(answer);
            }
            else
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var newAnswer = new Answer()
                {
                    Content = answerContent,
                    CreatedAt = DateTime.Now,
                    QuestionId = questionId,
                    userId = int.Parse(userId)
                };

                await _context.Answers.AddAsync(newAnswer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Questions");
        }
        catch (Exception e)
        {
            ViewBag.Error = "Bir hata oluştu";
            return View();
        }
    }

    [HttpPost , ActionName("Delete")]
    public async Task<IActionResult> Delete(int questionId)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var questions = await _context.Questions.FindAsync(questionId);
                if (questions == null)
                {
                    return NotFound();
                }

                _context.Questions.Remove(questions);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
        }

        return View();
    }
}