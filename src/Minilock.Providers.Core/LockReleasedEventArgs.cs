using System;

namespace Minilock.Providers.Core
{
    public class LockReleasedEventArgs : EventArgs
    {
        public string ClusterName { get; }
        
        public LockReleasedEventArgs(string sampleClusterName)
        {
            ClusterName = sampleClusterName;
        }
    }
}