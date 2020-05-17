using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Minilock.Providers.Redis
{
    public class MinilockRedisProviderConfigurator
    {
        private readonly IList<RedisInstance> _redisInstances;

        public IReadOnlyCollection<RedisInstance> RedisInstances =>
            new ReadOnlyCollection<RedisInstance>(_redisInstances);

        public bool HasRedisInstance => _redisInstances.Any();
        public TimeSpan WaitTime { get; set; } = new TimeSpan(0, 0, 1);
        public TimeSpan RetryTime { get; set; } = new TimeSpan(0, 0, 0);
        public CancellationToken? CancellationToken { get; set; }

        public MinilockRedisProviderConfigurator()
        {
            _redisInstances = new List<RedisInstance>();
        }

        public MinilockRedisProviderConfigurator AddRedisInstance(RedisInstance redisInstance)
        {
            _redisInstances.Add(redisInstance);
            return this;
        }

        public MinilockRedisProviderConfigurator WithWaitTime(TimeSpan duration)
        {
            WaitTime = duration;
            return this;
        }

        public MinilockRedisProviderConfigurator WithRetryTime(TimeSpan duration)
        {
            RetryTime = duration;
            return this;
        }

        public MinilockRedisProviderConfigurator WithCancellationToken(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
            return this;
        }

        internal MinilockRedisConfiguration Build()
        {
            if (!HasRedisInstance)
            {
                throw new ArgumentException("At least one redis instance configuration required");
            }
            
            var result = new MinilockRedisConfiguration
            {
                RedisInstances = RedisInstances,
                WaitTime = WaitTime,
                RetryTime = RetryTime,
                CancellationToken = CancellationToken
            };
            return result;
        }
    }
}