using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Minilock.Abstractions;

namespace Minilock.SampleApp
{
    public class MinilockHostService : IHostedService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IMinilockClusterStatusTracker _minilockClusterStatusTracker;

        public MinilockHostService(IHostApplicationLifetime appLifetime,
            IMinilockClusterStatusTracker minilockClusterStatusTracker)
        {
            _appLifetime = appLifetime;
            _minilockClusterStatusTracker = minilockClusterStatusTracker;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _minilockClusterStatusTracker.Watch();
        }

        private void OnStopping()
        {
            _minilockClusterStatusTracker.Close();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}