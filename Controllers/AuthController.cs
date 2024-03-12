using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Repositories;
using TonaWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using System.Text;

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
            string? result = await _authRepository.CreateUserAsync(user);
            if (result != null){
                ViewBag.result = result;
                return View(user); 
            }
            TempData["RegistrationSuccessMessage"] = "Registration successful! You can now log in.";
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
            var cookieOptions = new CookieOptions{};
            Response.Cookies.Append("email", Email, cookieOptions);
            return RedirectToAction("Index", "Home", userdb);
        }
        else{
            ViewBag.login = "wrong";
            return NotFound();
        }
    }

    public IActionResult Logout()
    {
        Response.Cookies.Delete("email");
        return RedirectToAction("Login", "Auth");
    }
}
