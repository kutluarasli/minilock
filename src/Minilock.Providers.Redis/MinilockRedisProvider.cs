using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Minilock.Providers.Core;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

namespace Minilock.Providers.Redis
{
    public class MinilockRedisProvider : IMinilockProvider
    {
        private readonly MinilockRedisConfiguration _configuration;
        private readonly RedLockFactory _redLockFactory;
        
        public MinilockRedisProvider(MinilockRedisProviderConfigurator configurator)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            _configuration = configurator.Build();

            _redLockFactory = InitializeRedLockFactory(_configuration);
        }
        
        private static RedLockFactory InitializeRedLockFactory(MinilockRedisConfiguration configuration)
        {
            var endpoints = configuration
                .RedisInstances
                .Select(redisInstance => new RedLockEndPoint(new DnsEndPoint(redisInstance.Address, redisInstance.Port))
                {
                    Password = redisInstance.AuthKey
                })
                .ToList();
                
            var redLockFactory = RedLockFactory.Create(endpoints);
            return redLockFactory;
        }
        
        public async Task<LockReference> LockAsync(string clusterName)
        {
            IRedLock redLock;
            do
            {
                redLock = await _redLockFactory.CreateLockAsync(clusterName, 
                    TimeSpan.MaxValue,
                    _configuration.WaitTime,
                    _configuration.RetryTime,
                    _configuration.CancellationToken);
                
            } while (!redLock.IsAcquired && !_configuration.CancellationRequested);

            return new RedLockReference(redLock);
        }

        public void Unlock(LockReference lockReference)
        {
            if (!(lockReference is RedLockReference redLockReference))
            {
                throw new ArgumentException("Only type of RedisLockReference can be used");
            }

            if (!redLockReference.LockAcquired)
            {
                return;
            }

            redLockReference.Release();
        }
        
        public void Dispose()
        {
            _redLockFactory?.Dispose();
        }
    }
}