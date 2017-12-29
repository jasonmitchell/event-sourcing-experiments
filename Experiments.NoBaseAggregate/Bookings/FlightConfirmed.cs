namespace Experiments.NoBaseAggregate.Bookings
{
    public class FlightConfirmed
    {
        public string FlightNumber { get; }
        public string DepartureAirport { get; }
        public string DestinationAirport { get; }

        public FlightConfirmed(string flightNumber, string departureAirport, string destinationAirport)
        {
            this.FlightNumber = flightNumber;
            this.DepartureAirport = departureAirport;
            this.DestinationAirport = destinationAirport;
        }
    }
}