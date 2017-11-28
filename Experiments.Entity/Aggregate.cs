using System;
using System.Collections.Generic;

namespace Experiments.Entity
{
    public abstract class Aggregate
    {
        private readonly Dictionary<Type, Action<object>> _handlers = new Dictionary<Type, Action<object>>();
        private readonly Queue<object> _uncommittedEvents = new Queue<object>();
        private int _version;

        public string Id { get; protected set; }
        public int Version => _version;
        public IEnumerable<object> UncommittedEvents => _uncommittedEvents;
        public IEnumerable<Type> EventTypes => _handlers.Keys;

        protected void Given<TEvent>(Action<TEvent> handler)
        {
            _handlers.Add(typeof(TEvent), x => handler((TEvent)x));
        }

        protected void Then<TEvent>(TEvent e)
        {
            Apply(e);
            _uncommittedEvents.Enqueue(e);
        }

        public void Apply(object e)
        {
            var type = e.GetType();
            if (_handlers.ContainsKey(type))
            {
                _handlers[type](e);
            }

            _version++;
        }

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }
    }
}