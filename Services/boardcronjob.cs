using TonaWebApp.Repositories;

namespace TonaWebApp.Services;

public class BoardCronJob(ILogger<BoardCronJob> logger, BoardRepository boardRepository) : BackgroundService
{
    private readonly ILogger<BoardCronJob> _logger = logger;

    private readonly BoardRepository _boardRepository = boardRepository;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Cronjob executed at: {time}", DateTimeOffset.Now);
            await _boardRepository.CheckBoardsOpenAsync();
            await _boardRepository.CheckBoardsExpiredAsync();
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}