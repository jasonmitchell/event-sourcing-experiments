using System;
using System.Collections.Generic;

namespace Experiments.ExternalAggregateState.Bookings
{
    public class Booking
    {
        private readonly State _state;

        public Booking(State state)
        {
            _state = state;
        }

        public static IEnumerable<object> Open()
        {
            var bookingReference = Guid.NewGuid().ToString("n");
            yield return new BookingOpened(bookingReference);
        }
        
        public IEnumerable<object> RequestFlight(string flightNumber, string departureAirport, string destinationAirport)
        {
            yield return new FlightRequested(flightNumber, departureAirport, departureAirport);
        }

        public IEnumerable<object> ConfirmFlight()
        {
            yield return new FlightConfirmed(_state.FlightDetails.Number,
                                             _state.FlightDetails.DepartureAirport,
                                             _state.FlightDetails.DestinationAirport);
        }
    }
}