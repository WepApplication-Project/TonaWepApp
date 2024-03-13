using MongoDB.Driver;
using TonaWebApp.Models;

namespace TonaWebApp.Config;

public class MongoDBContext
{
    private readonly IMongoDatabase _database;

    public MongoDBContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    public IMongoCollection<Board> Boards => _database.GetCollection<Board>("Boards");
    public IMongoCollection<Notification> Notifications => _database.GetCollection<Notification>("Notifications");

    public async Task InitializeUserDataAsync()
    {
        var usersCollection = _database.GetCollection<User>("Users");
        var existingUsersCount = await usersCollection.CountDocumentsAsync(_ => true);

        if (existingUsersCount == 0)
        {
            var users = new List<User>
                {
                    new() { FirstName = "User1", LastName = "Example" , Email = "user1@example.com", Password = "" },
                    new() { FirstName = "User2", LastName = "Example", Email = "user2@example.com", Password = "" }
                };

            await usersCollection.InsertManyAsync(users);
        }
    }

    public async Task InitializeBoardDataAsync()
    {
        var boardCollection = _database.GetCollection<Board>("Boards");
        var existingBoardsCount = await boardCollection.CountDocumentsAsync(_ => true);

        if (existingBoardsCount == 0)
        {
            var users = new List<Board>{
                new()
                {
                    Auther = new User { FirstName = "User1", LastName = "Example" , Email = "user1@example.com", Password = "" },
                    Name = "Project A",
                    Description = "Description for Project A",
                    MaxMember = 10,
                    IsActive = true,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow.AddDays(10),
                    MeetingTime = DateTime.UtcNow.AddDays(15),
                    Tag = "Tag1",
                    MemberList =
                    {

                    },
                    Place = "ECC"
                },
                new()
                {
                    Auther = new User { FirstName = "User1", LastName = "Example" , Email = "user1@example.com", Password = "" },
                    Name = "Project B",
                    Description = "Description for Project B",
                    MaxMember = 15,
                    IsActive = true,
                    StartTime = DateTime.UtcNow.AddDays(5),
                    EndTime = DateTime.UtcNow.AddDays(35),
                    MeetingTime = DateTime.UtcNow.AddDays(15),
                    Tag = "Tag2",
                    MemberList =
                    {

                    },
                    Place = "HCRL"
                }
            };


            await boardCollection.InsertManyAsync(users);
        }

    }

    public async Task InitializeNotificationDataAsync()
    {
        var notificationCollection = _database.GetCollection<Notification>("Notifications");
        var existingNotificationsCount = await notificationCollection.CountDocumentsAsync(_ => true);

        if (existingNotificationsCount == 0)
        {
            var notifications = new List<Notification>{
            new()
            {
                Title = "Notification 1 Title",
                SubTitle = "Notification 1 SubTitle",
                BoardId = "",
                User = new User { FirstName = "User2", LastName = "Example" , Email = "user2@example.com", Password = "" },
                IsReaded = false
            },
            new()
            {
                Title = "Notification 2 Title",
                SubTitle = "Notification 2 SubTitle",
                BoardId = "",
                User = new User { FirstName = "User1", LastName = "Example" , Email = "user1@example.com", Password = "" },
                IsReaded = false
            }
        };

            await notificationCollection.InsertManyAsync(notifications);
        }
    }

}
