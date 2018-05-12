using System;

namespace Experiments.ExternalAggregateState.Bookings
{
    public class BookingOpened
    {
        public string Reference { get; }
        
        public BookingOpened(string reference)
        {
            Reference = reference;
        }
    }
}