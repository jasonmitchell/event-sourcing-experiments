using System;
using System.Collections.Generic;

namespace Experiments.NoBaseAggregate
{
    public class EventSourceContext
    {
        private readonly Dictionary<Type, Action<EventSourceContext, object>> handlers = new Dictionary<Type, Action<EventSourceContext, object>>();
        private readonly Queue<object> recordedEvents;

        public EventSourceContext()
        {
            recordedEvents = new Queue<object>();
        }
        
        public EventSourceContext Given<TEvent>(Action<TEvent> handler)
        {
            handlers.Add(typeof(TEvent), (ctx, e) => handler((TEvent)e));
            return this;
        }
        
        public EventSourceContext Given<TEvent>(Action<EventSourceContext, TEvent> handler)
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