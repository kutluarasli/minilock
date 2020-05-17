using System;
using System.Threading.Tasks;

namespace Minilock.Abstractions
{
    public interface IMinilockClusterCoordinator : IDisposable
    {
        Task Start();
        void Close();
        ClusterInformation ClusterInformation { get; }
    }
}