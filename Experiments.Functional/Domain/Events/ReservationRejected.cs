using System;

namespace Experiments.Functional.Domain.Events
{
    public class ReservationRejected
    {
        public Guid ReservationId { get; }
        public Guid TicketId { get; }
        public int QuantityRequested { get; }
        public DateTime RejectedOn { get; }

        public ReservationRejected(Guid reservationId, Guid ticketId, int quantityRequested, DateTime rejectedOn)
        {
            ReservationId = reservationId;
            TicketId = ticketId;
            QuantityRequested = quantityRequested;
            RejectedOn = rejectedOn;
        }
    }
}