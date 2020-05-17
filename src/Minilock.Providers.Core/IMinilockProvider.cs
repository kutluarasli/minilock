using System;

namespace Minilock.Providers.Core
{
    public interface IMinilockProvider
    {
        bool Lock(string clusterName, string hostName);
        
        void Unlock(string clusterInformationClusterName, string hostName);
        
        event EventHandler<LockReleasedEventArgs> LockReleased;
    }
}