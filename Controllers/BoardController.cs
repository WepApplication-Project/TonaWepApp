using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Interface;
using TonaWebApp.Models;
using TonaWebApp.Repositories;

namespace TonaWebApp.Controllers;

public class BoardController(BoardRepository boardRepository, AuthRepository authRepository, IEmailService emailService, NotificationRepository notificationRepository) : Controller
{
    private readonly BoardRepository _boardRepository = boardRepository;

    private readonly AuthRepository _authRepository = authRepository;

    private readonly NotificationRepository _notificationRepository = notificationRepository;

    private readonly IEmailService _emailService = emailService;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var boardList = await _boardRepository.GetAllBoardAsync();
        return View(boardList);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(string Id)
    {
        var email = Request.Cookies["email"];
        if (!string.IsNullOrEmpty(email))
        {
            var user = await _authRepository.GetUserByEmailAsync(email);
            var board = await _boardRepository.GetBoardByIdAsync(Id);
            var boardAndUser = new BoardAndUserModel
            {
                User = user,
                Board = board
            };

            return View(boardAndUser);
        }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> CreateBoard()
    {
        var email = Request.Cookies["email"];
        if (!string.IsNullOrEmpty(email))
        {
            var user = await _authRepository.GetUserByEmailAsync(email);
            var boardAndUser = new BoardAndUserModel
            {
                User = user,
            };

            return View(boardAndUser);
        }
        return Unauthorized();
    }

    [HttpGet]
    public IActionResult EditBoard()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateBoard(Board board)
    {

        if (ModelState.IsValid)
        {
            var email = Request.Cookies["email"];
            if (!string.IsNullOrEmpty(email))
            {
                var user = await _authRepository.GetUserByEmailAsync(email);
                if (user != null)
                {
                    board.Auther = user;
                    await _boardRepository.CreateBoardAsync(board);
                    return RedirectToAction("Index", "home");
                }
            }
            else
            {
                return Unauthorized();
            }
        }
        return RedirectToAction("CreateBoard", "Board");
    }

    [HttpPost]
    public async Task<IActionResult> EditBoard(string Id, Board board)
    {
        if (Id != board.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _boardRepository.UpdateBoardAsync(board);
            return RedirectToAction("Index", "Home");
        }
        return View(board);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteBoard(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var board = await _boardRepository.GetBoardByIdAsync(id);
        if (board == null)
        {
            return NotFound();
        }

        await _boardRepository.DeleteBoardAsync(id);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> SignInToBoard(string email, string id)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Detail", "Board", new { Id = id });
        }
        var user = await _authRepository.GetUserByEmailAsync(email);
        var board = await _boardRepository.GetBoardByIdAsync(id);
        if (board == null || user == null)
        {
            return NotFound();
        }
        await _boardRepository.AddUserToBoard(user, board);
        if (user != null && user.Id != null && board != null)
        {
            var notification = new Notification
            {
                Title = "User joined your board!",
                SubTitle = $"{board.Name} User Is {user.FirstName} {user.LastName}",
                BoardId = board.Id!,
                User = board.Auther!
            };
            await _notificationRepository.AddNotificationAsync(notification);
        }
        // await _emailService.SendEmailAsync(board.Auther!.Email, board.Name, board.Description);
        return RedirectToAction("Detail", "Board", new { Id = id });
    }

    [HttpPost]
    public async Task<IActionResult> SignOutToBoard(string email, string id)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(id))
        {
            return RedirectToAction("Detail", "Board", new { Id = id });
        }

        var user = await _authRepository.GetUserByEmailAsync(email);
        var board = await _boardRepository.GetBoardByIdAsync(id);
        if (board == null || user == null)
        {
            return NotFound();
        }
        await _boardRepository.DeleteUserInBoard(user, board);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> OpenBoard(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            var board = await _boardRepository.GetBoardByIdAsync(id);
            if (board != null)
            {
                await _boardRepository.OpenBoardStatus(board);
            }
        }
        return RedirectToAction("Detail", "Board", new { Id = id });
    }

    [HttpPost]
    public async Task<IActionResult> CloseBoard(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            var board = await _boardRepository.GetBoardByIdAsync(id);
            if (board != null)
            {
                await _boardRepository.CloseBoardStatus(board);
                foreach(var user in board.MemberList)
                {
                    var notification = new Notification
                    {
                        Title = "Board Is Closed",
                        SubTitle = $"{board.Name} By Auther Is {board.Auther!.FirstName} {board.Auther!.LastName}",
                        BoardId = board.Id!,
                        User = user
                    };
                    await _notificationRepository.AddNotificationAsync(notification);
                }
            }
        }
        return RedirectToAction("Detail", "Board", new { Id = id });
    }
}