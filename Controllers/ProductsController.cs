using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Controllers;

public class ProductsController : Controller
{
    private readonly PM_Context _context;

    public ProductsController(PM_Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? categoryId)
    {
        var categories = await _context.Categories.ToListAsync();
        var products = _context.Products.AsQueryable();

        if (categoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == categoryId.Value);
        }

        var category = _context.Categories.Find(categoryId);

        var productsInfo = new ProductsInfo()
        {
            Category = categories,
            Products = products.ToList(),
            CategoryName = category?.Name
        };

        return View(productsInfo);
    }


    public async Task<IActionResult> Details(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var product = await _context.Products.FindAsync(id);

        var questions = await _context.Questions
            .Where(q => q.ProductId == id)
            .ToListAsync();

        var reviews = await _context.Reviews
            .Where(r => r.ProductId == id)
            .Include(r => r.User) // Kullanıcı bilgilerini dahil et
            .ToListAsync();

        var questionsId = questions.Select(q => q.Id).ToList();

        var answers = await _context.Answers
            .Where(a => questionsId.Contains(a.QuestionId))
            .ToListAsync();

        double ratingAverage = 0;
        if (reviews.Any())
        {
            ratingAverage = reviews.Average(r => r.Rating);
        }

        var ratingCounts = reviews
            .GroupBy(r => r.Rating)
            .ToDictionary(g => g.Key, g => g.Count());


        ViewBag.RatingAverage = ratingAverage;
        ViewBag.RatingCounts = ratingCounts;
        ViewBag.Questions = questions;
        ViewBag.Reviews = reviews;
        ViewBag.Answers = answers;

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }


    [HttpPost]
    public async Task<IActionResult> AskQuestions(string PQuestion, int ProductId)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                var question = new Questions()
                {
                    UserId = int.Parse(userId),
                    Question = PQuestion,
                    ProductId = ProductId,
                    CreatedAt = DateTime.Now
                };

                await _context.Questions.AddAsync(question);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Products", new { id = ProductId });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Soru Sorarken Bir Hata Oluştu.");
            }
        }
        else
        {
            ModelState.AddModelError("", "Model geçersiz.");
        }

        return View("ProductDetails");
    }

    public async Task<IActionResult> Reviews(string Review, int Rating, int ProductId)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                var product = _context.Products
                    .FirstOrDefault(p => p.Id == ProductId);

                var review = new Reviews()
                {
                    UserId = int.Parse(userId),
                    ProductId = ProductId,
                    Rating = Rating,
                    Review = Review,
                    CreatedAt = DateTime.Now,
                    Products = product
                };

                await _context.Reviews.AddAsync(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Products", new { id = ProductId });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "İnceleme Eklenemedi" + e.Message);
            }
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Answer(string answer, int id, int productId)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                var answers = new Answer()
                {
                    Content = answer,
                    CreatedAt = DateTime.Now,
                    QuestionId = id,
                    userId = int.Parse(userId),
                };

                await _context.Answers.AddAsync(answers);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Products", new { id = productId });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Soru cevaplanamadı." + e.Message);
            }
        }

        return RedirectToAction("Index", "Home");
    }
}