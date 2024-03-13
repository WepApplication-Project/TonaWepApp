// using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TonaWebApp.Models;
using TonaWebApp.Repositories;

namespace WebApplication1.Controllers
{
    public class HistoryController(BoardRepository boardRepository, AuthRepository authRepository) : Controller
    {
        private readonly BoardRepository _boardRepository = boardRepository;

        private readonly AuthRepository _authRepository = authRepository;

        public async Task<IActionResult> Upcoming()
        {
            var email = Request.Cookies["email"];
            if (!string.IsNullOrEmpty(email))
            {
                var user = await _authRepository.GetUserByEmailAsync(email);
                if (user != null)
                {
                    var boardList = await _boardRepository.GetBoardHistoryOpenAsync(user);
                    var homeIndexViewModel = new HomeIndexViewModel
                    {
                        User = user,
                        Boards = boardList,
                        SelectedTag = "",
                        TagsList = []
                    };
                    return View(homeIndexViewModel);
                }
            }
            return View();
        }
        public async Task<IActionResult> Completed()
        {
            var email = Request.Cookies["email"];
            if (!string.IsNullOrEmpty(email))
            {
                var user = await _authRepository.GetUserByEmailAsync(email);
                if (user != null)
                {
                    var boardList = await _boardRepository.GetBoardHistoryCloseAsync(user);
                    var homeIndexViewModel = new HomeIndexViewModel
                    {
                        User = user,
                        Boards = boardList,
                        SelectedTag = "",
                        TagsList = []
                    };
                    return View(homeIndexViewModel);
                }
            }
            return View();
        }
    }
}
