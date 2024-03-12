using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Models;
using TonaWebApp.Repositories;

namespace TonaWebApp.Controllers;

public class BoardController(BoardRepository boardRepository, AuthRepository authRepository) : Controller
{
    private readonly BoardRepository _boardRepository = boardRepository;

    private readonly AuthRepository _authRepository = authRepository;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var boardList = await _boardRepository.GetAllBoardAsync();
        return View(boardList);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(string Id)
    {
        var board = await _boardRepository.GetBoardByIdAsync(Id);
        return View(board);
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
                    board.Auther = user.Id;
                    await _boardRepository.CreateBoardAsync(board);
                    return RedirectToAction("Index", "home");
                }
            }
            else
            {
                return Unauthorized();
            }
        }
        return RedirectToAction("Create", "Board");
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
    public async Task<IActionResult> SignInToBoard(User user, string id)
    {
        if (id == null || user == null)
        {
            return View();
        }
        var board = await _boardRepository.GetBoardByIdAsync(id);
        if (board == null)
        {
            return NotFound();
        }

        await _boardRepository.AddUserToBoard(user, board);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> SignOutToBoard(User user, string id)
    {
        if (id == null || user == null)
        {
            return View();
        }

        var board = await _boardRepository.GetBoardByIdAsync(id);
        if (board == null)
        {
            return NotFound();
        }

        await _boardRepository.DeleteUserInBoard(user, board);

        return RedirectToAction("Index", "Home");
    }

}