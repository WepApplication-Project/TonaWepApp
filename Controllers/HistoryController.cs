// using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Controllers
{
    public class HistoryController : Controller
    {
        public IActionResult Upcoming()
        {
           
            return View();
        }
        public IActionResult Completed()
        {
           
            return View();
        }
    }
}
