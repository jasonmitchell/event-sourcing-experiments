using System.Collections.Generic;

namespace Experiments.NoBaseAggregate
{
    public interface IEventSource
    {
        void RestoreFromEvents(IEnumerable<object> events);
        object[] TakeEvents();
    }
}