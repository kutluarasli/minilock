namespace Minilock.Abstractions
{
    public interface IMinilockClusterStatusTrackerFactory
    {
        IMinilockClusterStatusTracker CreateStatusTracker(ClusterInformation clusterInformation);
    }
}