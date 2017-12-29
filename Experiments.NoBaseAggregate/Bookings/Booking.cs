using System;
using System.Collections.Generic;

namespace Experiments.NoBaseAggregate.Bookings
{
    public class Booking : IEventSource
    {
        private readonly EventSourcerThing _eventSourcerThing;

        public Guid Id { get; private set; }
        public string FlightNumber { get; private set; }
        public string DepartureAirport { get; private set; }
        public string DestinationAirport { get; private set; }
        public bool FlightConfirmed { get; private set; }

        private Booking()
        {
            _eventSourcerThing = new EventSourcerThing()
                .Given<BookingOpened>(e => Id = e.Id)
                .Given<FlightRequested>(e =>
                {
                    FlightNumber = e.FlightNumber;
                    DepartureAirport = e.DepartureAirport;
                    DestinationAirport = e.DestinationAirport;
                })
                .Given<FlightConfirmed>(_ => FlightConfirmed = true);
        }

        void IEventSource.RestoreFromEvents(IEnumerable<object> events) => _eventSourcerThing.Replay(events);
        object[] IEventSource.TakeEvents() => _eventSourcerThing.Reset();

        public static Booking Open(Guid id)
        {
            var booking = new Booking();
            booking._eventSourcerThing.Then(new BookingOpened(id));

            return booking;
        }

        public void RequestFlight(string flightNumber, string departureAirport, string destinationAirport)
        {
            if (FlightConfirmed) throw new InvalidOperationException("Flight already confirmed");
            _eventSourcerThing.Then(new FlightRequested(flightNumber, departureAirport, destinationAirport));
        }

        public void ConfirmFlight()
        {
            _eventSourcerThing.Then(new FlightConfirmed(FlightNumber, DepartureAirport, DestinationAirport));
        }
    }
}