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
        var existingUsersCount = await boardCollection.CountDocumentsAsync(_ => true);

        if (existingUsersCount == 0)
        {
            var users = new List<Board>{
                new() {
                    Auther = new User { FirstName = "User1", LastName = "Example" , Email = "user1@example.com", Password = "" },
                    Name = "Project A",
                    Description = "Description for Project A",
                    MaxMember = 10,
                    IsActive = true,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow.AddDays(30),
                    Tag = "Tag1",
                    MemberList =
                    {

                    }
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
                    Tag = "Tag2",
                    MemberList =
                    {

                    }
                }
            };


            await boardCollection.InsertManyAsync(users);
        }

    }
}
