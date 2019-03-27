using System;
using System.Collections.Generic;
using Experiments.Functional.Domain.Commands;
using Experiments.Functional.Domain.Events;

namespace Experiments.Functional.Domain
{
    public class Reservation
    {
        private readonly ReservationState _state;

        public Reservation(ReservationState state)
        {
            _state = state;
        }
        
        public IEnumerable<object> Handle(RequestTickets command)
        {
            var reservationId = Guid.NewGuid();
            var requestedOn = DateTime.UtcNow;

            yield return new TicketsRequested(reservationId, command.TicketId, command.QuantityRequested, requestedOn);
        }

        public IEnumerable<object> Handle(ConfirmReservation _)
        {
            var confirmedOn = DateTime.UtcNow;
            yield return new ReservationConfirmed(_state.Id, _state.TicketId, _state.QuantityReserved, confirmedOn);
        }

        public IEnumerable<object> Handle(RejectReservation _)
        {
            var rejectedOn = DateTime.UtcNow;
            yield return new ReservationRejected(_state.Id, _state.TicketId, _state.QuantityRequested, rejectedOn);
        }
    }
}