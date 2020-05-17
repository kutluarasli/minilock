using System;
using System.Threading.Tasks;
using Minilock.Abstractions;
using Minilock.Providers.Core;

namespace Minilock
{
    public class MinilockClusterCoordinator : IMinilockClusterCoordinator, IMinilockClusterStatusTracker
    {
        // Dependencies
        private readonly IMinilockProvider _provider;
        
        public event EventHandler<ClusterStatusChangedArgs> ClusterStatusChanged;
        
        // Fields & Properties
        public bool IsMaster => _lockReference?.LockAcquired == true;
        private LockReference _lockReference;
        public ClusterInformation ClusterInformation { get; }

        public MinilockClusterCoordinator(IMinilockProvider provider, ClusterInformation clusterInformation)
        {
            _provider = provider;
            
            ClusterInformation = clusterInformation;
        }

        public async Task Start()
        {
            _lockReference = await _provider.LockAsync(ClusterInformation.ClusterName);
            if (_lockReference.LockAcquired)
            {
                ClusterStatusChanged?.Invoke(this, new ClusterStatusChangedArgs(_lockReference.LockAcquired));
            }
        }

        public void Close()
        {
            if (_lockReference.LockAcquired)
            {
                _provider.Unlock(_lockReference);
            }
        }

        public void Dispose()
        {
            _provider?.Dispose();
        }
    }
}