using System;
using System.Collections.Generic;

namespace Experiments.NoBaseAggregate
{
    // TODO: Better name
    public class EventSourcerThing
    {
        private readonly Dictionary<Type, Action<EventSourcerThing, object>> handlers = new Dictionary<Type, Action<EventSourcerThing, object>>();
        private readonly Queue<object> recordedEvents;

        public EventSourcerThing()
        {
            recordedEvents = new Queue<object>();
        }
        
        public EventSourcerThing Given<TEvent>(Action<TEvent> handler)
        {
            handlers.Add(typeof(TEvent), (ctx, e) => handler((TEvent)e));
            return this;
        }
        
        public EventSourcerThing Given<TEvent>(Action<EventSourcerThing, TEvent> handler)
        {
            handlers.Add(typeof(TEvent), (ctx, e) => handler(ctx, (TEvent)e));
            return this;
        }

        public void Replay(IEnumerable<object> events)
        {
            foreach (var e in events)
            {
                Apply(e);
            }
        }
        
        public void Then<TEvent>(TEvent e)
        {
            Apply(e);
            recordedEvents.Enqueue(e);
        }

        private void Apply(object e)
        {
            var type = e.GetType();
            if (handlers.ContainsKey(type))
            {
                handlers[type](this, e);
            }
        }

        public object[] Reset()
        {
            var events = recordedEvents.ToArray();
            recordedEvents.Clear();
            
            return events;
        }
    }
}