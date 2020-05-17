using System;
using Minilock.Providers.Core;
using RedLockNet;

namespace Minilock.Providers.Redis
{
    public class RedLockReference : LockReference
    {
        private readonly IRedLock _redLock;

        public override bool LockAcquired => _redLock.IsAcquired;

        public RedLockReference(IRedLock redLock)
        {
            _redLock = redLock ?? throw new ArgumentNullException(nameof(redLock));
        }

        public void Release()
        {
            _redLock.Dispose();
        }
    }
}