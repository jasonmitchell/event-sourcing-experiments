using System;

namespace Experiments.NoBaseAggregate.Bookings
{
    public class BookingOpened
    {
        public Guid Id { get; }
        
        public BookingOpened(Guid id)
        {
            Id = id;
        }
    }
}