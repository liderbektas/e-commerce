using Microsoft.AspNetCore.Mvc;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly PM_Context _context = new();

    public IActionResult Index()
    {
        var categories = _context.Categories.ToList();
        return View(categories);
    }

    public IActionResult Create()
    {
        
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category category)
    {
        if (ModelState.IsValid)
        {
            try
            {
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

    public IActionResult Edit(int id)
    {
        var category = _context.Categories.Find(id);
        if (category == null)
        {
            return NotFound();
        }
        
        return View(category);
    }

    [HttpPost]
    public IActionResult Edit(int id, Category category)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction("Edit");
            }
            catch (Exception e)
            {
                ViewBag.Error = "Güncelleme işlemi yapılamadı." + e.Message;
            }
        }
        return View(category);
    }

    public IActionResult Delete(int id)
    {
        var category = _context.Categories.Find(id);
        return View(category);
    }
    
}