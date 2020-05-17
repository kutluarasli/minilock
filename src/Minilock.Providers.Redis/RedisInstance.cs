using System;

namespace Minilock.Providers.Redis
{
    public class RedisInstance
    {
        public string Address { get; }
        public int Port { get; }
        public string AuthKey { get;}
        
        public string FullAddress =>  $"{Address}:{Port}";

        public RedisInstance(string address, int port = 6379, string authKey = null)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (port <= 0)
            {
                throw new ArgumentException(nameof(port));
            }
            
            Address = address;
            Port = port;
            AuthKey = authKey;
        }
    }
}