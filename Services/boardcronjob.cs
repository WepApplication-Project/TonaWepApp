using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
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
            // var boardList = await _boardRepository.GetAllBoardAsync();
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}