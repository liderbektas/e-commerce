using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProductManagament_MVC.Controllers;

[Authorize]
public class AreaController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
