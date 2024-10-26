using Microsoft.AspNetCore.Mvc;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly PM_Context _context;

    public CategoryController(PM_Context context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        ViewData["ActivePage"] = "Category";
        var categories = _context.Categories.ToList();
        return View(categories);
    }
    
    [HttpPost]
    public IActionResult Create(string Name , string Description)
    {
        if (ModelState.IsValid)
        {
            try
            {

                var category = new Category()
                {
                    Name = Name,
                    Description = Description,
                    CreatedAt = DateTime.Now
                };

                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index" , "Category");
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
        }

        return View();
    }

    [HttpPost]
    public IActionResult Edit(int id, string Name , string Description)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var existCategory = _context.Categories.FirstOrDefault(c => c.Id == id);
                if (existCategory == null)
                {
                    return NotFound();
                }

                existCategory.Name = Name;
                existCategory.Description = Description;
                
                _context.Categories.Update(existCategory);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.Error = "Güncelleme işlemi yapılamadı." + e.Message;
            }
        }
        return View();
    }
    
    [HttpPost , ActionName("Delete")]
    public IActionResult Delete(int id)
    {
        if (ModelState.IsValid)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            try
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
        }
        
        return RedirectToAction("Index");
    }
    
}