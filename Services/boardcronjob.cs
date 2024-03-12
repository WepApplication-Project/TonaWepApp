using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace TonaWebApp.Services;

public class BoardCronJob(ILogger<BoardCronJob> logger) : BackgroundService
{
    private readonly ILogger<BoardCronJob> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Cronjob executed at: {time}", DateTimeOffset.Now);
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}