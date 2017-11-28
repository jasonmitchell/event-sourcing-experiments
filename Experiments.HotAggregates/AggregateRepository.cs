using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Experiments.HotAggregates
{
    public class AggregateRepository
    {
        private readonly AggregateCache cache;
        private readonly Func<string, object[]> streamReader;
        private readonly Action<string, object[]> streamWriter;

        public AggregateRepository(Func<string, object[]> streamReader, Action<string, object[]> streamWriter, int maxInMemoryCount = 100)
        {
            this.streamReader = streamReader;
            this.streamWriter = streamWriter;

            cache = new AggregateCache(maxInMemoryCount);
        }

        public TAggregate Load<TAggregate>(string aggregateId) where TAggregate : Aggregate
        {
            var stream = StreamName<TAggregate>(aggregateId);
            var cachedAggregate = cache.GetOrDefault(stream);
            if (cachedAggregate != null)
            {
                return (TAggregate)cachedAggregate;
            }

            var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);
            var events = streamReader(StreamName<TAggregate>(aggregateId));
            foreach (var e in events)
            {
                aggregate.Apply(e);
            }
            
            cache.Add(stream, aggregate);

            return aggregate;
        }

        public void Save<TAggregate>(TAggregate aggregate) where TAggregate : Aggregate
        {
            streamWriter(StreamName<TAggregate>(aggregate.Id), aggregate.UncommittedEvents.ToArray());
            aggregate.ClearUncommittedEvents();
        }

        private static string StreamName<TAggregate>(string aggregateId) where TAggregate : Aggregate
        {
            return $"{typeof(TAggregate).Name}-{aggregateId}";
        }
    }
}