using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Minilock.Abstractions;

namespace Minilock.SampleApp
{
    public class WorkerService : BackgroundService
    {
        private readonly IMinilockClusterStatusTracker _minilockClusterStatusTracker;

        public WorkerService(IMinilockClusterStatusTracker minilockClusterStatusTracker)
        {
            _minilockClusterStatusTracker = minilockClusterStatusTracker;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"Status is IsMaster={_minilockClusterStatusTracker.IsMaster}");
                await Task.Delay(1000);
            }
        }
    }
}