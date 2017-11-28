using System;

namespace Experiments.Entity.Bookings
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