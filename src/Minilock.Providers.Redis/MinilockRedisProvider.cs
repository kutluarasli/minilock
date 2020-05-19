using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Minilock.Providers.Core;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

namespace Minilock.Providers.Redis
{
    public class MinilockRedisProvider : IMinilockProvider
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly MinilockRedisConfiguration _configuration;
        private readonly RedLockFactory _redLockFactory;
        
        public MinilockRedisProvider(MinilockRedisProviderConfigurator configurator, ILoggerFactory loggerFactory = null)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            _loggerFactory = loggerFactory;

            _configuration = configurator.Build();
            
            _redLockFactory = InitializeRedLockFactory(_configuration, loggerFactory);
        }
        
        private static RedLockFactory InitializeRedLockFactory(MinilockRedisConfiguration configuration, ILoggerFactory loggerFactory)
        {
            var endpoints = configuration
                .RedisInstances
                .Select(redisInstance => new RedLockEndPoint(new DnsEndPoint(redisInstance.Address, redisInstance.Port))
                {
                    Password = redisInstance.AuthKey,
                    RedisDatabase = 0,
                    RedisKeyFormat = "minilock:{0}",
                })
                .ToList();

            var redLockFactory = RedLockFactory.Create(endpoints, loggerFactory);
            return redLockFactory;
        }
        
        public async Task<LockReference> LockAsync(string clusterName)
        {
            IRedLock redLock;
            do
            {
                redLock = await _redLockFactory.CreateLockAsync(clusterName, 
                    _configuration.LockDuration,
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