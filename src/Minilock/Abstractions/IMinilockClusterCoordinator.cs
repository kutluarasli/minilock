namespace Minilock.Abstractions
{
    public interface IMinilockClusterCoordinator
    {
        void Start();
        void Close();
        ClusterInformation ClusterInformation { get; }
    }
}