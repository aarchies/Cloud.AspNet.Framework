using Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Internal;
using System;

namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Options
{
    public class RedisServerOptions
    {
        public RedisServerOptions()
        {
            Host = Const.DEFAULT_REDIS_HOST;
            Port = Const.DEFAULT_REDIS_PORT;
        }

        public RedisServerOptions(string host)
        {
            //GlasssixArgumentException.ThrowIfNullOrWhiteSpace(host);

            var lastIndex = host.LastIndexOf(':');
            if (lastIndex > 0 && host.Length > lastIndex + 1 && int.TryParse(host.AsSpan(lastIndex + 1), out var port))
            {
                Host = host.Substring(0, lastIndex);
                Port = port;
            }

            if (string.IsNullOrEmpty(Host))
            {
                Host = host;
                Port = Const.DEFAULT_REDIS_PORT;
            }
        }

        public RedisServerOptions(string host, int port)
        {
            //GlasssixArgumentException.ThrowIfNullOrWhiteSpace(host);

            if (port <= 0)
                throw new ArgumentOutOfRangeException(nameof(port), $"{nameof(port)} must be greater than 0");

            Host = host;
            Port = port;
        }

        public string Host { get; set; }

        public int Port { get; set; }
    }
}