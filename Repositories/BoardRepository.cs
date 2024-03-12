using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TonaWebApp.Models;
using TonaWebApp.Config;

namespace TonaWebApp.Repositories;

public class BoardRepository(MongoDBContext context)
{
    private readonly IMongoCollection<Board> _boards = context.Boards;

    public async Task<List<Board>> GetAllBoardAsync()
    {
        try
        {
            var boardList = await _boards.Find(_ => true).ToListAsync();
            return boardList;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while fetching users: {ex}");
            return [];
        }
    }

    public async Task<Board> GetBoardByIdAsync(string Id)
    {
        var filter = Builders<Board>.Filter.Eq(u => u.Id, Id);
        return await _boards.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<Board>> GetBoardHistoryOpenAsync(User user)
    {
        var allBoards = await GetAllBoardAsync();

        var filteredBoards = allBoards.Where(board =>
            board.IsActive &&
            board.MemberList.Any(member => member.Id == user.Id) // MemberList contains the user
        ).ToList();

        return filteredBoards;
    }

    public async Task<List<Board>> GetBoardHistoryCloseAsync(User user)
    {
        var allBoards = await GetAllBoardAsync();

        var filteredBoards = allBoards.Where(board =>
            !board.IsActive &&
            board.MemberList.Any(member => member.Id == user.Id) // MemberList contains the user
        ).ToList();

        return filteredBoards;
    }

    public async Task CreateBoardAsync(Board board)
    {
        await _boards.InsertOneAsync(board);
    }

    public async Task UpdateBoardAsync(Board board)
    {
        var filter = Builders<Board>.Filter.Eq(existingBoard => existingBoard.Id, board.Id);
        await _boards.ReplaceOneAsync(filter, board);
    }

    public async Task DeleteBoardAsync(string id)
    {
        await _boards.DeleteOneAsync(board => board.Id == id);
    }

    public async Task AddUserToBoard(User user, Board board)
    {
        if (board.MemberList.Any(m => m.Id == user.Id))
        {
            throw new InvalidOperationException("User is already a member of the board.");
        }

        board.AddMember(user);

        await UpdateBoardAsync(board);
    }

    public async Task DeleteUserInBoard(User user, Board board)
    {
        if (user == null || board == null)
        {
            return;
        }

        int index = board.MemberList.FindIndex(u => u.Id == user.Id);

        if (index != -1)
        {
            board.MemberList.RemoveAt(index);
            await UpdateBoardAsync(board);
        }
    }


    public async Task OpenBoardStatus(Board board)
    {
        if (board == null)
        {
            return;
        }

        board.IsActive = true;

        await UpdateBoardAsync(board);
    }

    public async Task CloseBoardStatus(Board board)
    {
        if (board == null)
        {
            return;
        }

        board.IsActive = false;

        board.MemberList.Clear();

        var tempUserList = board.MemberList.Take(board.MaxMember - 1).ToList();
        foreach (var user in tempUserList)
        {
            board.MemberList.Add(user);
        }

        await UpdateBoardAsync(board);
    }

}