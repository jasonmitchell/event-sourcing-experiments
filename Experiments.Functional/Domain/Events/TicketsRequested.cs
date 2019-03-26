using System;

namespace Experiments.Functional.Domain.Events
{
    public class TicketsRequested
    {
        public Guid ReservationId { get; }
        public Guid TicketId { get; }
        public int QuantityRequested { get; }
        public DateTime RequestedOn { get; }

        public TicketsRequested(Guid reservationId, Guid ticketId, int quantityRequested, DateTime requestedOn)
        {
            ReservationId = reservationId;
            TicketId = ticketId;
            QuantityRequested = quantityRequested;
            RequestedOn = requestedOn;
        }
    }
}