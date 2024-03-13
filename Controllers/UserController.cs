using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Models;
using TonaWebApp.Repositories;

namespace TonaWebApp.Controllers;

public class UserController(AuthRepository authRepository) : Controller
{
    private readonly AuthRepository _authRepository = authRepository;

    public async Task<IActionResult> Profile()
    {
        var email = Request.Cookies["email"];
        if (!string.IsNullOrEmpty(email))
        {
            var user = await _authRepository.GetUserByEmailAsync(email);

            return View(user);
        }
        return Unauthorized();
    }

    public IActionResult EditProfile()
    {
        return View();
    }
}
