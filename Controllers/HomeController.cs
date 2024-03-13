using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Models;
using TonaWebApp.Repositories;

namespace TonaWebApp.Controllers;

public class HomeController(AuthRepository authRepository, BoardRepository boardRepository, NotificationRepository notificationRepository) : Controller
{
    private readonly AuthRepository _authRepository = authRepository;
    private readonly BoardRepository _boardRepository = boardRepository;

    private readonly NotificationRepository _notificationRepository = notificationRepository;

    [HttpGet]
    public async Task<IActionResult> Index(string tag="all")
    {
        var boardList = await _boardRepository.GetAllBoardAsync();
        ViewBag.tag = tag;
        var email = Request.Cookies["email"];
        if (!string.IsNullOrEmpty(email))
        {
            var user = await _authRepository.GetUserByEmailAsync(email);
            if (user != null && user.Id != null)
            {
                if(tag == "user"){
                    var homeIndexViewModel = new HomeIndexViewModel
                    {
                    User = user,
                    Boards = boardList.Where(b => b.Auther!.Id == user.Id).ToList(),
                    SelectedTag = tag,
                    TagsList = ["love", "food", "study", "travel", "sport", "game"]
                    };
                    return View(homeIndexViewModel);
                }
                else{
                    var homeIndexViewModel = new HomeIndexViewModel
                    {
                    User = user,
                    Boards = (tag == "all") ? boardList : boardList.Where(b => b.Tag == tag).ToList(),
                    SelectedTag = tag,
                    TagsList = ["love", "food", "study", "travel", "sport", "game"]
                    };
                    return View(homeIndexViewModel);
                }
                
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
