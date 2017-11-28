namespace Experiments.HotAggregates.Sales
{
    public class TicketSaleScheduled
    {
        public string TicketId { get; }
        public int Allocation { get; }

        public TicketSaleScheduled(string ticketId, int allocation) 
        {
            TicketId = ticketId;
            Allocation = allocation;
        }
    }
}