using Microsoft.AspNetCore.Mvc;

namespace ProductManagament_MVC.Controllers;

public class ContactController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}