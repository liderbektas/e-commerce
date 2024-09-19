using Microsoft.AspNetCore.Mvc;


namespace ProductManagament_MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class MainController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}