using System;
using System.Collections.Generic;

namespace Experiments.NoBaseAggregate.Bookings
{
    public class Booking : IEventSource
    {
        private readonly EventSourcerThing eventSourcerThing;

        public Guid Id { get; private set; }
        public Flight Flight { get; private set; }

        private Booking()
        {
            eventSourcerThing = new EventSourcerThing()
                .Given<BookingOpened>(e => Id = e.Id)
                .Given<FlightRequested>((ctx, e) =>
                {
                    Flight = new Flight(ctx);
                    Flight.Given(e);
                })
                .Given<FlightConfirmed>(e => Flight.Given(e));
        }

        void IEventSource.RestoreFromEvents(IEnumerable<object> events) => eventSourcerThing.Replay(events);
        object[] IEventSource.TakeEvents() => eventSourcerThing.Reset();

        public static Booking Open(Guid id)
        {
            var booking = new Booking();
            booking.eventSourcerThing.Then(new BookingOpened(id));

            return booking;
        }

        public void RequestFlight(string flightNumber, string departureAirport, string destinationAirport)
        {
            if (Flight?.Confirmed == true) throw new InvalidOperationException("Flight already confirmed");
            eventSourcerThing.Then(new FlightRequested(flightNumber, departureAirport, destinationAirport));
        }
    }
}