using System;
using System.Collections.Generic;

namespace Experiments.HotAggregates.Sales
{
    public class TicketSale : Aggregate
    {
        private int allocation;
        private int reserved;

        private int Availability => allocation - reserved;

        private TicketSale()
        {
            Given<TicketSaleScheduled>(e =>
            {
                Id = e.TicketId;
                allocation = e.Allocation;
            });

            Given<TicketReservationConfirmed>(e =>
            {
                reserved += e.Quantity;
            });
        }

        public TicketSale(string id, int allocation) : this()
        {
            Then(new TicketSaleScheduled(id, allocation));
        }

        public void Reserve(int quantity)
        {
            if (Availability < quantity) throw new InvalidOperationException("Insufficient availability");
            Then(new TicketReservationConfirmed(Id, Guid.NewGuid().ToString(), quantity));
        }
    }
}