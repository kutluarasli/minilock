using System;
using System.Threading.Tasks;
using System.Timers;
using Minilock.Abstractions;
using Minilock.Providers.Core;

namespace Minilock
{
    public sealed class MinilockClusterStatusTracker : IMinilockClusterStatusTracker
    {
        // Dependencies
        private readonly IMinilockProvider _provider;
        
        // Fields 
        private bool _wasMaster;
        private Timer _statusTrackingTimer;
        private LockReference _lockReference;
        
        // Properties 
        public ClusterInformation ClusterInformation { get; }
        public bool IsMaster => _lockReference.LockAcquired;
        
        //Events
        public event EventHandler<ClusterStatusChangedArgs> ClusterStatusChanged;

        public MinilockClusterStatusTracker(IMinilockProvider provider, ClusterInformation clusterInformation)
        {
            _provider = provider;
            _lockReference = new LockReference(false);
            ClusterInformation = clusterInformation;
        }
        
        public void Watch(int pollingInterval = 100)
        {
            Claim().ConfigureAwait(false);
            InitStatusTrackingTimer(pollingInterval);
        }

        internal async Task Claim()
        {
            _lockReference = await _provider.LockAsync(ClusterInformation.ClusterName);
        }

        public void Close()
        {
            _statusTrackingTimer?.Stop();
            if (_lockReference?.LockAcquired == true)
            {
                _provider.Unlock(_lockReference);
            }
        }

        public void Dispose()
        {
            _provider?.Dispose();
            _statusTrackingTimer?.Dispose();
        }
        
        private void InitStatusTrackingTimer(int pollingInterval)
        {
            _statusTrackingTimer = new Timer()
            {
                Interval = pollingInterval
            };
            _statusTrackingTimer.Elapsed += StatusTrackingTimerOnElapsed;
            _statusTrackingTimer.Start();
        }
        
        private void StatusTrackingTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            CheckStatus();
        }

        internal void CheckStatus()
        {
            var isMasterNow = IsMaster;
            if (_wasMaster != isMasterNow)
            {
                _wasMaster = isMasterNow;
                OnClusterStatusChanged(new ClusterStatusChangedArgs(isMasterNow));
            }            
        }
        
        private void OnClusterStatusChanged(ClusterStatusChangedArgs e)
        {
            ClusterStatusChanged?.Invoke(this, e);
        }
    }
}