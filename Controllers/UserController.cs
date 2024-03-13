using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Models;
using TonaWebApp.Repositories;

namespace TonaWebApp.Controllers;

public class UserController(AuthRepository authRepository, CloudinaryController cloudinaryController, BoardRepository boardRepository) : Controller
{
    private readonly AuthRepository _authRepository = authRepository;

    private readonly CloudinaryController _cloudinaryController = cloudinaryController;

    private readonly BoardRepository _boardRepository = boardRepository;

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var email = Request.Cookies["email"];
        if (!string.IsNullOrEmpty(email))
        {
            var user = await _authRepository.GetUserByEmailAsync(email);

            return View(user);
        }
        return Unauthorized();
    }

    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        var email = Request.Cookies["email"];
        if (!string.IsNullOrEmpty(email))
        {
            var user = await _authRepository.GetUserByEmailAsync(email);

            return View(user);
        }
        return Unauthorized();
    }

    [HttpPost]
public async Task<IActionResult> UpdateProfile(string FirstName, string LastName, string Email, IFormFile profilePicture)
{
    var email = Request.Cookies["email"];
    if (!string.IsNullOrEmpty(email))
    {
        var user = await _authRepository.GetUserByEmailAsync(email);
        if (user != null)
        {
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Email = Email;
            Console.WriteLine("updating...");
            Console.WriteLine("check : " + profilePicture);
            if (profilePicture != null && profilePicture.Length > 0)
            {
                Console.WriteLine("profile : " + profilePicture);
                string? imageUrl = await _cloudinaryController.UploadImageAsync(profilePicture);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    user.ImageUrl = imageUrl;
                }
            }

            var updatedUser = await _authRepository.UpdateUserAsync(user);
            if (updatedUser != null)
            {
                var boardList = await _boardRepository.GetAllBoardAsync();

                var homeIndexViewModel = new HomeIndexViewModel
                {
                    User = updatedUser,
                    Boards = boardList,
                    SelectedTag = "",
                    TagsList = [ "love", "food", "study", "travel", "sport", "game" ]
                };

                return RedirectToAction("Index", "Home", homeIndexViewModel);
            }
        }
        else
        {
            return NotFound();
        }
    }
    return Unauthorized();
}


}
