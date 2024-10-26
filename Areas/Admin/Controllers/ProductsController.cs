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
        
        var categories = await _context.Categories.ToListAsync();
        ViewData["Categories"] = new SelectList(categories, "Id", "Name");
        
        return View(products);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(string Name, string Description, decimal Price, int CategoryId, int Stock, IFormFile? img)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var currentUserName = User.Identity.Name;
                var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.userName == currentUserName);

                if (currentUser != null)
                {
                    var product = new Products
                    {
                        Name = Name,
                        Description = Description,
                        Price = Price,
                        UserId = currentUser.Id,
                        Stock = Stock,
                        CreatedAt = DateTime.Now,
                        CategoryId = CategoryId
                    };

                    if (img != null)
                    {
                        var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/");
                        var filePath = Path.Combine(folder, img.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await img.CopyToAsync(stream);
                        }

                        product.Img = img.FileName;
                    }

                    _context.Products.Add(product);
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
    
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(int id, string Name, string Description, decimal Price, int Stock)
    {
        if (ModelState.IsValid)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            
            existingProduct.Name = Name;
            existingProduct.Description = Description;
            existingProduct.Price = Price;
            existingProduct.Stock = Stock; 

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View();
    }
    
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id )
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