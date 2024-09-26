using Microsoft.AspNetCore.Mvc;

namespace ProductManagament_MVC.Controllers;

public class Success : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}