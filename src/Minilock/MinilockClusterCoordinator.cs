using System;
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
        private bool _isMaster;
        public ClusterInformation ClusterInformation { get; }

        public bool IsMaster
        {
            get => _isMaster;
            private set
            {
                if (value != _isMaster)
                {
                    _isMaster = value;
                    ClusterStatusChanged?.Invoke(this, new ClusterStatusChangedArgs(_isMaster));
                }
            }
        }
        
        public MinilockClusterCoordinator(IMinilockProvider provider, ClusterInformation clusterInformation)
        {
            _provider = provider;
            
            ClusterInformation = clusterInformation;
        }

        public void Start()
        {
            _provider.LockReleased += OnLockReleased;

            ClaimMasterRole();
        }

        private void ClaimMasterRole()
        {
            IsMaster = _provider.Lock(ClusterInformation.ClusterName, ClusterInformation.HostName);
        }

        private void OnLockReleased(object sender, LockReleasedEventArgs args)
        {
            ClaimMasterRole();
        }

        public void Close()
        {
            _provider.LockReleased -= OnLockReleased;
            _provider.Unlock(ClusterInformation.ClusterName, ClusterInformation.HostName);
        }
    }
}