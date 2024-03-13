using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Models;
using TonaWebApp.Repositories;

namespace TonaWebApp.Controllers;

public class HomeController(AuthRepository authRepository, BoardRepository boardRepository) : Controller
{
    private readonly AuthRepository _authRepository = authRepository;
    private readonly BoardRepository _boardRepository = boardRepository;

    [HttpGet]
    public async Task<IActionResult> Index(string tag)
    {
        var boardList = await _boardRepository.GetAllBoardAsync();
        ViewBag.tag = tag;
        var email = Request.Cookies["email"];
        if (!string.IsNullOrEmpty(email))
        {
            var user = await _authRepository.GetUserByEmailAsync(email);
            if (user != null)
            {
                var homeIndexViewModel = new HomeIndexViewModel
                {
                    User = user,
                    Boards = string.IsNullOrEmpty(tag) ? boardList : boardList.Where(b => b.Tag == tag).ToList(),
                    SelectedTag = tag,
                    TagsList = ["love", "food", "study", "travel", "sport", "game"]
                };

                return View(homeIndexViewModel);
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
