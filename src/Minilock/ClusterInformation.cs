using System;
using System.Diagnostics.CodeAnalysis;

namespace Minilock
{
    public class ClusterInformation
    {
        public string ClusterName { get; }
        public string HostName { get; }
        
        public ClusterInformation(string clusterName, string hostName)
        {
            if (string.IsNullOrWhiteSpace(clusterName))
            {
                throw new ArgumentNullException(nameof(clusterName));
            }

            if (string.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentNullException(nameof(hostName));
            }

            ClusterName = clusterName;
            HostName = hostName;
        }

        public ClusterInformation Clone()
        {
            var copy = new ClusterInformation(ClusterName, HostName);
            return copy;
        }
    }
}