using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TonaWebApp.Models;
using TonaWebApp.Repositories;


public class UserController : Controller
{
    public ActionResult Profile()
    {
        return View();
    }

    public ActionResult EditProfile()
    {
        return View();
    }

    public ActionResult Details(string id)
    {
        return View();
    }
}





// public class UserController : Controller
// {
//     // private readonly IMongoCollection<User> _usersCollection;

//     // public UsersController(IMongoDatabase database)
//     // {
//     //     // Replace "YourDatabaseName" with the actual name of your MongoDB database
//     //     _usersCollection = database.GetCollection<User>("Users");
//     // }

//     // GET: Users
//     public ActionResult Profile()
//     {
//         // var users = _usersCollection.Find(user => true).ToList();
//         return View();
//     }

//     // GET: Users/Details/{userId}
//     public ActionResult Details(string id)
//     {
//         // var user = _usersCollection.Find(u => u.Id == id).FirstOrDefault();
//         // if (user == null)
//         // {
//         //     return NotFound();
//         // }

//         return View();
//     }
// }

