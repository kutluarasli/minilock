using System;
using System.Collections.Generic;
using System.Threading;

namespace Minilock.Providers.Redis
{
    internal class MinilockRedisConfiguration
    {
        public IReadOnlyCollection<RedisInstance>? RedisInstances { get; set; }
        public TimeSpan WaitTime { get; set; }
        public TimeSpan RetryTime { get; set; }
        public CancellationToken? CancellationToken { get; set; }
        public TimeSpan LockDuration { get; set; }

        public bool CancellationRequested => CancellationToken?.IsCancellationRequested == true;
    }
}