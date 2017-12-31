using System.Collections.Generic;

namespace Experiments.NoBaseAggregate.Bookings
{
    public class Flight
    {
        private readonly EventSourcerThing eventSourcerThing;
        
        public string Number {get; private set;}
        public string DepartureAirport {get; private set;}
        public string DestinationAirport {get; private set;}
        public bool Confirmed { get; private set; }

        internal Flight(EventSourcerThing parentEventSourcer)
        {
            eventSourcerThing = parentEventSourcer.CreateChild()
                .Given<FlightRequested>(e =>
                {
                    Number = e.FlightNumber;
                    DepartureAirport = e.DepartureAirport;
                    DestinationAirport = e.DestinationAirport;
                })
                .Given<FlightConfirmed>(e => Confirmed = true);
        }
        
        public void Confirm()
        {
            eventSourcerThing.Then(new FlightConfirmed(Number, DepartureAirport, DestinationAirport));
        }
    }
}
