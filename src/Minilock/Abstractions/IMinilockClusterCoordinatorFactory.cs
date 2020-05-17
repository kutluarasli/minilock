namespace Minilock.Abstractions
{
    public interface IMinilockClusterCoordinatorFactory
    {
        IMinilockClusterCoordinator CreateCoordinator(ClusterInformation clusterInformation);
    }
}