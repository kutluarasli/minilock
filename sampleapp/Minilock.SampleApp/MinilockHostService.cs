using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Minilock.Abstractions;

namespace Minilock.SampleApp
{
    public class MinilockHostService : IHostedService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IMinilockClusterCoordinator _minilockClusterCoordinator;

        public MinilockHostService(IHostApplicationLifetime appLifetime,
            IMinilockClusterCoordinator minilockClusterCoordinator)
        {
            _appLifetime = appLifetime;
            _minilockClusterCoordinator = minilockClusterCoordinator;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _minilockClusterCoordinator.Start();
        }

        private void OnStopping()
        {
            _minilockClusterCoordinator.Close();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}