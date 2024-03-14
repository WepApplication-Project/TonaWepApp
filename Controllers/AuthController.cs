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
        var email = Request.Cookies["email"];
        if (!string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Index","Home");
        }
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
        var email = Request.Cookies["email"];
        if (!string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Index","Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequestModel)
    {
        var userdb = await _authRepository.GetUserByEmailAsync(loginRequestModel.Email);
        if (userdb.Password == loginRequestModel.Password)
        {
            var cookieOptions = new CookieOptions{};
            Response.Cookies.Append("email", loginRequestModel.Email, cookieOptions);
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
