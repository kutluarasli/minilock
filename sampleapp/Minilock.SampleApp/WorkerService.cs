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
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _minilockClusterStatusTracker.ClusterStatusChanged += (sender, args) =>
                Console.WriteLine($"Status is IsMaster={args.IsMaster}");
            
            return Task.CompletedTask;
        }
    }
}