using System;
using System.Collections.Generic;
using Experiments.Functional.Domain.Commands;
using Experiments.Functional.Domain.Events;

namespace Experiments.Functional.Domain
{
    public class Reservation
    {
        public static IEnumerable<object> Handle(ReservationState _, RequestTickets command)
        {
            var reservationId = Guid.NewGuid();
            var requestedOn = DateTime.UtcNow;

            yield return new TicketsRequested(reservationId, command.TicketId, command.QuantityRequested, requestedOn);
        }

        public static IEnumerable<object> Handle(ReservationState state, ConfirmReservation _)
        {
            var confirmedOn = DateTime.UtcNow;
            yield return new ReservationConfirmed(state.Id, state.TicketId, state.QuantityReserved, confirmedOn);
        }

        public static IEnumerable<object> Handle(ReservationState state, RejectReservation _)
        {
            var rejectedOn = DateTime.UtcNow;
            yield return new ReservationRejected(state.Id, state.TicketId, state.QuantityRequested, rejectedOn);
        }
    }
}