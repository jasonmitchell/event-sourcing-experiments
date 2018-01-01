using System;
using System.Collections.Generic;

namespace Experiments.NoBaseAggregate.Bookings
{
    public class Booking : IEventSource
    {
        private readonly EventSourceContext context;
        private Action<object> Then => context.Then;

        public Guid Id { get; private set; }
        public Flight Flight { get; private set; }

        private Booking()
        {
            context = new EventSourceContext()
                .Given<BookingOpened>(e => Id = e.Id)
                .Given<FlightRequested>((ctx, e) =>
                {
                    Flight = new Flight(ctx.Then);
                    Flight.Given(e);
                })
                .Given<FlightConfirmed>(e => Flight.Given(e));
        }

        void IEventSource.RestoreFromEvents(IEnumerable<object> events) => context.Replay(events);
        object[] IEventSource.TakeEvents() => context.Reset();

        public static Booking Open(Guid id)
        {
            var booking = new Booking();
            booking.Then(new BookingOpened(id));

            return booking;
        }

        public void RequestFlight(string flightNumber, string departureAirport, string destinationAirport)
        {
            if (Flight?.Confirmed == true) throw new InvalidOperationException("Flight already confirmed");
            Then(new FlightRequested(flightNumber, departureAirport, destinationAirport));
        }
    }
}