using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Models;
using TonaWebApp.Repositories;

namespace TonaWebApp.Controllers;

public class NotificationController(AuthRepository authRepository, BoardRepository boardRepository, NotificationRepository notificationRepository) : Controller
{
    private readonly AuthRepository _authRepository = authRepository;
    private readonly BoardRepository _boardRepository = boardRepository;

    private readonly NotificationRepository _notificationRepository = notificationRepository;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var email = Request.Cookies["email"];
        if (!string.IsNullOrEmpty(email))
        {
            var user = await _authRepository.GetUserByEmailAsync(email);
            if (user != null && user.Id != null)
            {
                var notificationList = await _notificationRepository.GetNotificationsByUserIdAsync(user.Id);
                
            }
        }
        else
        {
            return Unauthorized();
        }
        return View();
    }
}