using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Minilock.Abstractions;
using Minilock.Providers.Redis;

namespace Minilock.SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    ConfigureMinilock(services);
                    services.AddHostedService<MinilockHostService>();
                    services.AddHostedService<WorkerService>();
                });

        private static void  ConfigureMinilock(IServiceCollection serviceCollection)
        {
                  
            var redisProviderConfigurator = new MinilockRedisProviderConfigurator()
                .AddRedisInstance(new RedisInstance("localhost"))
                .WithLockDuration(TimeSpan.FromMinutes(1));
            var redisProvider = new MinilockRedisProvider(redisProviderConfigurator);
            var minilockClusterStatusTrackerFactory = new MinilockClusterStatusTrackerFactory(redisProvider);
            var minilockClusterStatusTracker =
                minilockClusterStatusTrackerFactory.CreateStatusTracker(new ClusterInformation("test-cluster", "host-1"));

            serviceCollection.AddSingleton(provider => minilockClusterStatusTracker);
        }
    }
}
