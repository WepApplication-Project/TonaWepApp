using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Repositories;
using TonaWebApp.Models;

namespace TonaWebApp.Controllers;

public class UserController : Controller
{
    private readonly UserRepository _userRepository;

    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return View("~/Views/Home/Index.cshtml", users);
    }

    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        if (ModelState.IsValid)
        {

            await _userRepository.CreateUserAsync(user);
            return RedirectToAction("Index", "Home");
        }
        return View(user);
    }
}
