namespace Experiments.HotAggregates.Sales
{
    public class TicketReservationConfirmed
    {
        public string TicketId { get; }
        public string ReservationId { get; }
        public int Quantity { get; }

        public TicketReservationConfirmed(string ticketId, string reservationId, int quantity) 
        {
            TicketId = ticketId;
            ReservationId = reservationId;
            Quantity = quantity;
        }
    }
}