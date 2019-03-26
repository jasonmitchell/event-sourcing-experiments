using System;

namespace Experiments.Functional.Domain.Commands
{
    public class RejectReservation
    {
        public Guid ReservationId { get; }

        public RejectReservation(Guid reservationId)
        {
            ReservationId = reservationId;
        }
    }
}