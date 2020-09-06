using System;
using Minilock.Abstractions;
using Minilock.Providers.Core;

namespace Minilock
{
    public class MinilockClusterStatusTrackerFactory : IMinilockClusterStatusTrackerFactory
    {
        private readonly IMinilockProvider _provider;

        public MinilockClusterStatusTrackerFactory(IMinilockProvider provider)
        {
            _provider = provider;
        }

        public IMinilockClusterStatusTracker CreateStatusTracker(ClusterInformation? clusterInformation)
        {
            if (clusterInformation == null)
            {
                throw new ArgumentNullException(nameof(clusterInformation));
            }

            var clusterCoordinator = new MinilockClusterStatusTracker(_provider, clusterInformation.Clone());
            return clusterCoordinator;
        }
    }
}