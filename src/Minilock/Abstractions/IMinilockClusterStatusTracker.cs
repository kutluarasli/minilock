namespace Minilock.Abstractions
{
    public interface IMinilockClusterStatusTracker
    {
        public bool IsMaster { get; }
    }
}