using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductsController : Controller
{
    private readonly PM_Context _context;

    public ProductsController(PM_Context context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        ViewData["ActivePage"] = "Products";
        var products = await _context.Products.Include(x => x.Category).ToListAsync();
        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Products products, IFormFile? img)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var currentUserName = User.Identity.Name;
                var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.userName == currentUserName);

                if (currentUser != null)
                {
                    if (img != null)
                    {
                        var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/");
                        using var stream = new FileStream(Path.Combine(folder, img.FileName), FileMode.Create);
                        await img.CopyToAsync(stream);
                        products.UserId = currentUser.Id;
                        products.Img = img.FileName;
                        products.CreatedAt = DateTime.Now;
                    }

                    _context.Products.Add(products);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ürün eklerken bir hata oluştu: " + ex.Message;
            }
        }
        else
        {
            ViewBag.Error = "Formda eksik veya geçersiz bilgiler var.";
        }
        
        return View(products);
    }
    
    public async Task<IActionResult> Edit(int id)
    {
        var products = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (products == null)
        {
           return NotFound();
        }

        return View(products);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Products product, int id , int Stock)
    {
        if (ModelState.IsValid)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Stock = Stock; 

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        return View(product);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var products = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (products == null)
        {
            NotFound();
        }

        return View(products);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var products = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (ModelState.IsValid)
        {
            try
            {
                _context.Products.Remove(products);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.Error = "Ürün silme işlemi başarısız oldu.";
            }
        }

        return View(products);
    }
}