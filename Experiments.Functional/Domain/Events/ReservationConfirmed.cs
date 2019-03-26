using System;

namespace Experiments.Functional.Domain.Events
{
    public class ReservationConfirmed
    {
        public Guid ReservationId { get; }
        public Guid TicketId { get; }
        public int QuantityReserved { get; }
        public DateTime ConfirmedOn { get; }

        public ReservationConfirmed(Guid reservationId, Guid ticketId, int quantityReserved, DateTime confirmedOn)
        {
            ReservationId = reservationId;
            TicketId = ticketId;
            QuantityReserved = quantityReserved;
            ConfirmedOn = confirmedOn;
        }
    }
}