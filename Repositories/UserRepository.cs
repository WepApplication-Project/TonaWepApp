using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TonaWebApp.Models;
using TonaWebApp.Config;

namespace TonaWebApp.Repositories;

public class AuthRepository(MongoDBContext context)
{
    private readonly IMongoCollection<User> _users = context.Users;

    public async Task<List<User>> GetAllUsersAsync()
    {
        try
        {
            var userList = await _users.Find(_ => true).ToListAsync();
            return userList;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while fetching users: {ex}");
            return [];
        }
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        return await _users.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateUserAsync(User user)
    {
        await _users.InsertOneAsync(user);
    }
}
