using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Interface;
using TonaWebApp.Models;
using TonaWebApp.Repositories;

namespace TonaWebApp.Controllers;

public class BoardController(BoardRepository boardRepository, AuthRepository authRepository, IEmailService emailService) : Controller
{
    private readonly BoardRepository _boardRepository = boardRepository;

    private readonly AuthRepository _authRepository = authRepository;

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
    public IActionResult CreateBoard()
    {
        return View();
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
        Console.WriteLine("user email : " + email);
        Console.WriteLine("board Id : " + id);
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
            }
        }
        return RedirectToAction("Detail", "Board", new { Id = id });
    }
}