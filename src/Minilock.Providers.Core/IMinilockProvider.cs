using System;
using System.Threading.Tasks;

namespace Minilock.Providers.Core
{
    public interface IMinilockProvider : IDisposable
    {
        Task<LockReference> LockAsync(string clusterName);
        
        void Unlock(LockReference lockReference);
    }
}