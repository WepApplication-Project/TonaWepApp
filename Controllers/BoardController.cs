using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Models;
using TonaWebApp.Repositories;

namespace TonaWebApp.Controllers;

public class BoardController(BoardRepository boardRepository) : Controller
{
    private readonly BoardRepository _boardRepository = boardRepository;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var boardList = await _boardRepository.GetAllBoardAsync();
        return View(boardList);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBoard(Board board)
    {
        if (ModelState.IsValid)
        {
            await _boardRepository.CreateBoardAsync(board);
            return RedirectToAction("Index", "home");
        }
        return View(board);
    }
}