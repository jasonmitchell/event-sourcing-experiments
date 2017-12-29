using System;
using System.Collections.Generic;

namespace Experiments.NoBaseAggregate
{
    // TODO: Better name
    public class EventSourcerThing
    {
        private readonly Dictionary<Type, Action<object>> _handlers = new Dictionary<Type, Action<object>>();
        private readonly Queue<object> _recordedEvents = new Queue<object>();

        public void Then<TEvent>(TEvent e)
        {
            _recordedEvents.Enqueue(e);
        }

        public EventSourcerThing Given<TEvent>(Action<TEvent> handler)
        {
            _handlers.Add(typeof(TEvent), x => handler((TEvent)x));
            return this;
        }
        
        public void Replay(IEnumerable<object> events)
        {
            foreach (var e in events)
            {
                var type = e.GetType();
                if (_handlers.ContainsKey(type))
                {
                    _handlers[type](e);
                }
            }
        }

        public object[] Reset()
        {
            var events = _recordedEvents.ToArray();
            _recordedEvents.Clear();
            
            return events;
        }
    }
}