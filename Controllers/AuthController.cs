using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Repositories;
using TonaWebApp.Models;

namespace TonaWebApp.Controllers;

public class AuthController(AuthRepository authRepository) : Controller
{
    private readonly AuthRepository _authRepository = authRepository;


    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register([Bind("FirstName,LastName,Phone,Email,Password")] User user)
    {
        if (ModelState.IsValid)
        {
            await _authRepository.CreateUserAsync(user);
            return RedirectToAction("Login");
        }
        return View(user);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string Email, string Password)
    {
        var userdb = await _authRepository.GetUserByEmailAsync(Email);
        if (userdb.Password == Password)
        {
            HttpContext.Session.SetString("email", Email);
            return RedirectToAction("Index", "Home", userdb);
        }
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("email");
        return RedirectToAction("Login", "Auth");
    }
}
