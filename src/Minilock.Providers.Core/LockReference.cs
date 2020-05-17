using System.Data;

namespace Minilock.Providers.Core
{
    public class LockReference
    {
        public virtual bool LockAcquired { get; } = false;

        public LockReference(bool lockAcquired)
        {
            LockAcquired = lockAcquired;
        }

        protected LockReference()
        {
        }
    }
}