namespace Experiments.NoBaseAggregate.Bookings
{
    public class FlightRequested
    {
        public string FlightNumber { get; }
        public string DepartureAirport { get; }
        public string DestinationAirport { get; }

        public FlightRequested(string flightNumber, string departureAirport, string destinationAirport)
        {
            this.FlightNumber = flightNumber;
            this.DepartureAirport = departureAirport;
            this.DestinationAirport = destinationAirport;
        }
    }
}