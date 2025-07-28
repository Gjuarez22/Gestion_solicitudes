using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GestionSolicitud.HelperClasses;
using GestionSolicitud.HelperClasses.Interfaces;


namespace GestionSolicitud.HelperClasses
{
    public class BackgroundQueueWorker : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;

        private readonly ILogger<BackgroundQueueWorker> _logger;

        public BackgroundQueueWorker(IBackgroundTaskQueue taskQueue, ILogger<BackgroundQueueWorker> logger)
        {
            _taskQueue = taskQueue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background Queue Worker is running.");
            await BackgroundProcessing(stoppingToken);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await (await _taskQueue.DequeueAsync(stoppingToken))(stoppingToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error occurred executing task work item.");
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background Queue Worker is stopping.");
            await base.StopAsync(stoppingToken);
        }
    }
}
