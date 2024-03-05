using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Models;
using TonaWebApp.Repositories;

namespace TonaWebApp.Controllers;

public class HomeController(AuthRepository authRepository) : Controller
{
    private readonly AuthRepository _authRepository = authRepository;

    public async Task<IActionResult> Index()
    {
        var email = HttpContext.Session.GetString("email");
        if (!string.IsNullOrEmpty(email))
        {
            var user = await _authRepository.GetUserByEmailAsync(email);
            if (user != null)
            {
                return View(user);
            }
        }
        return RedirectToAction("Login", "Auth");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
