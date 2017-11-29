using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Experiments.HotAggregates
{
    public class AggregateCache
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        
        private readonly ConcurrentDictionary<string, (Aggregate aggregate, DateTime cacheTime)> cache = new ConcurrentDictionary<string, (Aggregate aggregate, DateTime cacheTime)>();
        private readonly int maxCount;
        private readonly TimeSpan maxLifespan;
        private readonly TimeSpan pruneInterval;

        public AggregateCache(int maxCount, TimeSpan maxLifespan, TimeSpan pruneInterval)
        {
            this.maxCount = maxCount;
            this.maxLifespan = maxLifespan;
            this.pruneInterval = pruneInterval;            

            BeginLifespanMonitoring();
        }

        private void BeginLifespanMonitoring()
        {
            Task.Run(async () =>
            {
                await Task.Delay(pruneInterval, cancellationTokenSource.Token);
                
                while (true)
                {
                    var expired = cache.Where(x => x.Value.cacheTime + maxLifespan < DateTime.UtcNow).Select(x => x.Key);
                    foreach (var e in expired)
                    {
                        cache.Remove(e, out var removedAggregate);
                    }
                    
                    await Task.Delay(pruneInterval, cancellationTokenSource.Token);
                }
            }, cancellationTokenSource.Token);
        }

        public void Add(string identifier, Aggregate aggregate)
        {
            if (cache.Count == maxCount)
            {
                var oldestItem = cache.OrderBy(x => x.Value.cacheTime).FirstOrDefault();
                if (oldestItem.Key != identifier)
                {
                    cache.Remove(oldestItem.Key, out var removedAggregate);
                }
            }
            
            cache.AddOrUpdate(identifier, (aggregate, DateTime.UtcNow), (key, value) => (value.aggregate, DateTime.UtcNow));
        }

        public Aggregate GetOrDefault(string identifier)
        {
            if (cache.ContainsKey(identifier))
            {
                var aggregate = cache[identifier].aggregate;
                cache.AddOrUpdate(identifier, (aggregate, DateTime.UtcNow), (key, value) => (value.aggregate, DateTime.UtcNow));
                
                return aggregate;
            }

            return null;
        }
    }
}