using System;
using System.Collections.Generic;

namespace Experiments.ExternalAggregateState.Bookings
{
    public class Service
    {
        public void Open()
        {
            var events = Booking.Open();
            
            // Save events
        }

        public void RequestFlight()
        {
            // Load events from a stream
            var pastEvents = new object[]
            {
                new BookingOpened(Guid.NewGuid().ToString("n"))
            };

            var state = Project(pastEvents);
            var booking = new Booking(state);
            var newEvents = booking.RequestFlight("AB123", "BHD", "LBA");
            
            // Save events
        }

        public void ConfirmFlight()
        {
            // Load events from a stream
            var pastEvents = new object[]
            {
                new BookingOpened(Guid.NewGuid().ToString("n")),
                new FlightRequested("AB123", "BHD", "LBA"), 
            };

            var state = Project(pastEvents);
            var booking = new Booking(state);
            var newEvents = booking.ConfirmFlight();
            
            // Save events
        }

        private State Project(IEnumerable<object> events)
        {
            var state = new State();

            foreach (var @event in events)
            {
                switch (@event)
                {
                    case BookingOpened e:
                        state.BookingReference = e.Reference;
                        break;
                    
                    case FlightRequested e:
                        state.FlightDetails = new State.Flight
                        {
                            Number = e.FlightNumber,
                            DepartureAirport = e.DepartureAirport,
                            DestinationAirport = e.DestinationAirport
                        };
                        break;
                    
                    case FlightConfirmed _:
                        state.FlightDetails.Confirmed = true;
                        break;
                }
            }

            return state;
        }
    }
}