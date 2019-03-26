using System;

namespace Experiments.Functional.Domain.Commands
{
    public class ConfirmReservation
    {
        public Guid ReservationId { get; }

        public ConfirmReservation(Guid reservationId)
        {
            ReservationId = reservationId;
        }
    }
}