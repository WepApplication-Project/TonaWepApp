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

    [HttpGet]
    public async Task<IActionResult> Detail(string Id)
    {
        var board = await _boardRepository.GetBoardByIdAsync(Id);
        return View(board);
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
}