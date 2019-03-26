using System;

namespace Experiments.Functional.Domain.Commands
{
    public class RequestTickets
    {
        public Guid TicketId { get; }
        public int QuantityRequested { get; }
        
        public RequestTickets(Guid ticketId, int quantityRequested)
        {
            TicketId = ticketId;
            QuantityRequested = quantityRequested;
        }
    }
}