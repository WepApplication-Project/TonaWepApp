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
                    new User { Username = "User1", Email = "user1@example.com" },
                    new User { Username = "User2", Email = "user2@example.com" }
                };

                await usersCollection.InsertManyAsync(users);
            }
        }
}
