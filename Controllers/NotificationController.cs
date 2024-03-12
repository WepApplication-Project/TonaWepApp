using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Models;
using TonaWebApp.Repositories;

namespace TonaWebApp.Controllers;

public class NotificationController(AuthRepository authRepository, BoardRepository boardRepository) : Controller
{
    private readonly AuthRepository _authRepository = authRepository;
    private readonly BoardRepository _boardRepository = boardRepository;

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}