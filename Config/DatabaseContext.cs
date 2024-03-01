using MongoDB.Driver;
using TonaWebApp.Models;

namespace TonaWebApp.Config;

public class MongoDBContext
{
    private IMongoDatabase _database;

    public MongoDBContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");

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
}
