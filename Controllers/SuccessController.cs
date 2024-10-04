using Microsoft.AspNetCore.Mvc;

namespace ProductManagament_MVC.Controllers;

public class SuccessController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}