using System;
using Minilock.Abstractions;
using Minilock.Providers.Core;

namespace Minilock
{
    public class MinilockClusterCoordinatorFactory : IMinilockClusterCoordinatorFactory
    {
        private readonly IMinilockProvider _provider;

        public MinilockClusterCoordinatorFactory(IMinilockProvider provider)
        {
            _provider = provider;
        }

        public IMinilockClusterCoordinator CreateCoordinator(ClusterInformation clusterInformation)
        {
            if (clusterInformation == null)
            {
                throw new ArgumentNullException(nameof(clusterInformation));
            }

            var clusterCoordinator = new MinilockClusterCoordinator(_provider, clusterInformation.Clone());
            return clusterCoordinator;
        }
    }
}