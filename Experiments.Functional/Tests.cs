using System;
using System.Linq;
using Experiments.Functional.Domain;
using Experiments.Functional.Domain.Commands;
using Experiments.Functional.Domain.Events;
using Xunit;

namespace Experiments.Functional
{
    public class Tests
    {
        [Fact]
        public void RaisesTicketsRequested()
        {
            var state = ReservationState.Initial;
            var command = new RequestTickets(Guid.NewGuid(), 4);
            var reservation = new Reservation(state);
            var events = reservation.Handle(command).ToList();

            Assert.Single(events);
            Assert.IsType<TicketsRequested>(events.Single());
        }

        [Fact]
        public void RaisesReservationConfirmed()
        {
            var reservationId = Guid.NewGuid();
            var history = new object[]
            {
                new TicketsRequested(reservationId, Guid.NewGuid(), 4, DateTime.UtcNow.AddMinutes(-2)),
            };

            var state = ReservationState.Apply(history);
            var command = new ConfirmReservation(reservationId);
            var reservation = new Reservation(state);
            var events = reservation.Handle(command).ToList();
            
            Assert.Single(events);
            Assert.IsType<ReservationConfirmed>(events.Single());
        }
    }
}