# minilock

**What is Minilock?**

Minilock is a minimalist library to create mutual exclusions in a distributed systems. Minilock itself aims to solve the problem where leader node is required. 

Under the hood, minilock uses provider abstraction to store mutex state. For now, only state implementation is available for Redis.

**More about Redis Provider**

Redis provider implementation is based on RedLock algorithm. Whoever gains the the lock over a Redis key (Cluster Name), is the owner of mutex (Leader)

Provider depends on external RedLock.Net library. https://github.com/samcook/RedLock.net

**Show me the code**


       //Configure provider
        var redisProviderConfigurator = new MinilockRedisProviderConfigurator()
            .AddRedisInstance(new RedisInstance("localhost"))
            .WithLockDuration(TimeSpan.FromMinutes(1));
        var redisProvider = new MinilockRedisProvider(redisProviderConfigurator);
            
        //Create an instance of tracker
        var minilockClusterStatusTrackerFactory = new MinilockClusterStatusTrackerFactory(redisProvider);
        var minilockClusterStatusTracker =
            minilockClusterStatusTrackerFactory.CreateStatusTracker(new ClusterInformation("test-cluster", "host-1"));

        //Watch for changes
        minilockClusterStatusTracker.ClusterStatusChanged += (sender, args) =>
            Console.WriteLine($"Am I the master now {args.IsMaster}"); 
        minilockClusterStatusTracker.Watch();

For more information, please check /sampleapp. 

