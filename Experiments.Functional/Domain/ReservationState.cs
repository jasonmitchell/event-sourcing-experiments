using System;
using System.Collections.Generic;
using Experiments.Functional.Domain.Events;

namespace Experiments.Functional.Domain
{
    public class ReservationState
    {
        public Guid Id { get; private set; }
        public ReservationStatus Status { get; private set; } 
        public Guid TicketId { get; private set; }
        public int QuantityRequested { get; private set; }
        public int QuantityReserved { get; private set; }
        
        public static ReservationState Initial { get; } = new ReservationState();
            
        // TODO: Consider making this immutable
        public static ReservationState Apply(IEnumerable<object> events) => Apply(Initial, events);
        
        public static ReservationState Apply(ReservationState state, IEnumerable<object> events)
        {
            foreach (var e in events)
            {
                Apply(state, e);
            }

            return state;
        }

        private static void Apply(ReservationState state, object @event)
        {
            switch (@event)
            {
                case TicketsRequested e:
                    state.Id = e.ReservationId;
                    state.Status = ReservationStatus.Requested;
                    state.TicketId = e.TicketId;
                    state.QuantityRequested = e.QuantityRequested;
                    break;

                case ReservationRejected _:
                    state.Status = ReservationStatus.Rejected;
                    break;

                case ReservationConfirmed e:
                    state.Status = ReservationStatus.Confirmed;
                    state.QuantityReserved = e.QuantityReserved;
                    break;
            }
        }
        
        public enum ReservationStatus
        {
            Requested,
            Rejected,
            Confirmed
        }
    }
}