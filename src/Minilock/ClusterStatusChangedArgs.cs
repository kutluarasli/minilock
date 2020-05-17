using System;

namespace Minilock
{
    public class ClusterStatusChangedArgs : EventArgs
    {
        public bool IsMaster { get; }
        
        public ClusterStatusChangedArgs(bool isMaster)
        {
            IsMaster = isMaster;
        }
    }
}