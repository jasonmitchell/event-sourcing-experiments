using System;

namespace Experiments.Entity.Bookings
{
    public class Booking : Aggregate
    {
        public Flight Flight {get; private set;}

        private Booking()
        {
            Given<BookingOpened>(e => Id = e.Id.ToString());

            Given<FlightRequested>(e =>
            {
                Flight = new Flight(Then);
                Flight.Given(e);
            });

            Given<FlightConfirmed>(e => Flight.Given(e));
        }

        public Booking(Guid id) : this()
        {
            Then(new BookingOpened(id));
        }

        public void RequestFlight(string flightNumber, string departureAirport, string destinationAirport)
        {
            if (Flight.Confirmed) throw new InvalidOperationException("Flight already confirmed");
            Then(new FlightRequested(flightNumber, departureAirport, destinationAirport));
        }
    }
}