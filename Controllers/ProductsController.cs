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

        var productsViewModel = new ProductsViewModel()
        {
            Category = categories,
            Products = products.ToList(),
            CategoryName = category?.Name
        };

        return View(productsViewModel);
    }
    
    public async Task<IActionResult> Details(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        var users = await _context.Users.ToListAsync();

        var questions = await _context.Questions
            .Where(q => q.ProductId == id)
            .ToListAsync();

        var reviews = await _context.Reviews
            .Where(r => r.ProductId == id)
            .Include(r => r.User)
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

        var viewModel = new ProductDetailsViewModel
        {
            Product = product,
            Questions = questions,
            Answers = answers,
            Reviews = reviews,
            RatingAverage = ratingAverage,
            RatingCounts = ratingCounts
        };

        return View(viewModel);
    }

    
    [HttpPost]
    public async Task<IActionResult> AskQuestions(string PQuestion, int ProductId)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == ProductId);

                if (userId == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                var question = new Questions()
                {
                    UserId = int.Parse(userId),
                    Question = PQuestion,
                    ProductId = ProductId,
                    Products = product,
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

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(userId));
                
                var review = new Reviews()
                {
                    UserId = int.Parse(userId),
                    User = user,
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
}