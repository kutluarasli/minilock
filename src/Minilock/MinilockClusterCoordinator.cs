using System;
using Minilock.Abstractions;
using Minilock.Providers.Core;

namespace Minilock
{
    public class MinilockClusterCoordinator : IMinilockClusterCoordinator
    {
        // Dependencies
        private readonly IMinilockProvider _provider;
        
        // Events
        public event EventHandler<ClusterStatusChangedArgs> ClusterStatusChanged;
        
        // Fields & Properties
        public ClusterInformation ClusterInformation { get; }
        private bool _isClaimSuccessful;

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
            var result = _provider.Lock(ClusterInformation.ClusterName, ClusterInformation.HostName);
            
            if (_isClaimSuccessful != result)
            {
                _isClaimSuccessful = result;
                
                ClusterStatusChanged?.Invoke(this, new ClusterStatusChangedArgs(_isClaimSuccessful));
            }
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