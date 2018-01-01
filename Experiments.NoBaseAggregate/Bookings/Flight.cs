using System;
using System.Collections.Generic;

namespace Experiments.NoBaseAggregate.Bookings
{
    public class Flight
    {
        private Action<object> Then { get; }
        
        public string Number {get; private set;}
        public string DepartureAirport {get; private set;}
        public string DestinationAirport {get; private set;}
        public bool Confirmed { get; private set; }

        internal Flight(Action<object> then)
        {
            Then = then;
        }

        public void Given(FlightRequested e) 
        {
            Number = e.FlightNumber;
            DepartureAirport = e.DepartureAirport;
            DestinationAirport = e.DestinationAirport;
        }

        public void Given(FlightConfirmed e) 
        {
            Confirmed = true;
        }
        
        public void Confirm()
        {
            Then(new FlightConfirmed(Number, DepartureAirport, DestinationAirport));
        }
    }
}
