using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Experiments.HotAggregates
{
    public class AggregateRepository
    {
        private readonly Dictionary<string, Aggregate> cache = new Dictionary<string, Aggregate>();
        private readonly Func<string, object[]> streamReader;
        private readonly Action<string, object[]> streamWriter;

        public AggregateRepository(Func<string, object[]> streamReader, Action<string, object[]> streamWriter)
        {
            this.streamReader = streamReader;
            this.streamWriter = streamWriter;
        }

        public TAggregate Load<TAggregate>(string aggregateId) where TAggregate : Aggregate
        {
            var stream = StreamName<TAggregate>(aggregateId);
            if (cache.ContainsKey(stream))
            {
                return (TAggregate)cache[stream];
            }

            var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);
            var events = streamReader(StreamName<TAggregate>(aggregateId));
            foreach (var e in events)
            {
                aggregate.Apply(e);
            }
            
            cache[stream] = aggregate;

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