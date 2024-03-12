using MongoDB.Driver;
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

    public async Task<string?> CreateUserAsync(User user)
    {
        var existingEmailUser = await _users.Find(u => u.Email == user.Email).FirstOrDefaultAsync();
        if (existingEmailUser != null)
        {
            return "Email already exists.";
        }

        var existingPhoneUser = await _users.Find(u => u.Phone == user.Phone).FirstOrDefaultAsync();
        if (existingPhoneUser != null)
        {
            return "Phone already exists.";
        }
        await _users.InsertOneAsync(user);
        return null;
    }
}
