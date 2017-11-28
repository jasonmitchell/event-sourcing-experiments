using System;
using System.Collections.Generic;
using System.Linq;

namespace Experiments.HotAggregates
{
    public class AggregateCache
    {
        private readonly Dictionary<string, (Aggregate aggregate, DateTime cacheTime)> cache = new Dictionary<string, (Aggregate aggregate, DateTime cacheTime)>();
        
        private readonly int maxCount;

        public AggregateCache(int maxCount)
        {
            this.maxCount = maxCount;
        }

        public void Add(string identifier, Aggregate aggregate)
        {
            if (cache.Count == maxCount)
            {
                var oldestItem = cache.OrderBy(x => x.Value.cacheTime).FirstOrDefault();
                if (oldestItem.Key != identifier)
                {
                    cache.Remove(oldestItem.Key);
                }
            }

            cache[identifier] = (aggregate, DateTime.UtcNow);
        }

        public Aggregate GetOrDefault(string identifier)
        {
            if (cache.ContainsKey(identifier))
            {
                return cache[identifier].aggregate;
            }

            return null;
        }
    }
}