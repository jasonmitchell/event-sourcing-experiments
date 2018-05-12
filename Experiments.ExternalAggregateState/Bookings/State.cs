namespace Experiments.ExternalAggregateState.Bookings
{
    // TODO: What to call this?
    public class State
    {
        public string BookingReference { get; set; }
        public Flight FlightDetails { get; set; }
        
        public class Flight
        {
            public string Number {get; set;}
            public string DepartureAirport {get; set;}
            public string DestinationAirport {get; set;}
            public bool Confirmed { get; set; }
        }
    }
}