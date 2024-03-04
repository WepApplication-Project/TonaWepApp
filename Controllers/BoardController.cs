using Microsoft.AspNetCore.Mvc;

namespace TonaWebApp.Controllers;

public class BoardController() : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}