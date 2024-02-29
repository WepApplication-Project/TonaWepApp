using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Repositories;

namespace TonaWebApp.Controllers;

public class UserController : Controller
{
    private readonly UserRepository _userRepository;

    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return View("~/Views/Home/Index.cshtml", users);
    }
}
