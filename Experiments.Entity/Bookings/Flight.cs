using System;

namespace Experiments.Entity.Bookings
{
    public class Flight : Entity
    {
        public string Number {get; private set;}
        public string DepartureAirport {get; private set;}
        public string DestinationAirport {get; private set;}
        public bool Confirmed { get; private set; }

        public Flight(Action<object> then) : base(then)
        {
            Given<FlightRequested>(e => 
            {
                Number = e.FlightNumber;
                DepartureAirport = e.DepartureAirport;
                DestinationAirport = e.DestinationAirport;
            });

            Given<FlightConfirmed>(e => Confirmed = true);
        }

        public void Confirm()
        {
            Then(new FlightConfirmed(Number, DepartureAirport, DestinationAirport));
        }
    }
}