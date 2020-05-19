using System;

namespace Minilock.Abstractions
{
    public interface IMinilockClusterStatusTracker : IDisposable
    {
        bool IsMaster { get; }

        void Watch(int pollingInterval = 100);
        
        void Close();
        
        ClusterInformation ClusterInformation { get; }

        event EventHandler<ClusterStatusChangedArgs> ClusterStatusChanged;
    }
}