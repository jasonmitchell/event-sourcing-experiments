using System;
using System.Collections.Generic;

namespace Experiments.Entity
{
    public abstract class Entity
    {
        private readonly Dictionary<Type, Action<object>> _handlers = new Dictionary<Type, Action<object>>();
        private readonly Action<object> _then;

        protected Entity(Action<object> then)
        {
            _then = then;
        }

        protected void Given<TEvent>(Action<TEvent> handler)
        {
            _handlers.Add(typeof(TEvent), x => handler((TEvent)x));
        }

        public void Given<TEvent>(TEvent e)
        {
            var type = e.GetType();
            if (_handlers.ContainsKey(type))
            {
                _handlers[type](e);
            }
        }

        protected void Then<TEvent>(TEvent e)
        {
            _then(e);
        }
    }
}