using System;
using Experiments.NoBaseAggregate.Bookings;
using FluentAssertions;
using Xunit;

namespace Experiments.NoBaseAggregate.Tests
{
    public class AggregateEventTests
    {
        [Fact]
        public void Rehydrates()
        {
            var events = new object[]
            {
                new BookingOpened(Guid.Parse("00000000-0000-0000-0000-000000000001")),
                new FlightRequested("AB123", "BHD", "LBA"),
                new FlightConfirmed("AB123", "BHD", "LBA"), 
            };

            var booking = (Booking)Activator.CreateInstance(typeof(Booking), true);
            booking.As<IEventSource>().RestoreFromEvents(events);

            booking.Id.Should().Be(Guid.Parse("00000000-0000-0000-0000-000000000001"));
            booking.Flight.Should().NotBeNull();
            booking.Flight.Number.Should().Be("AB123");
            booking.Flight.DepartureAirport.Should().Be("BHD");
            booking.Flight.DestinationAirport.Should().Be("LBA");
            booking.Flight.Confirmed.Should().BeTrue();
        }

        [Fact]
        public void TracksEvents()
        {
            var booking = Booking.Open(Guid.Parse("00000000-0000-0000-0000-000000000001"));
            booking.RequestFlight("AB123", "BHD", "LBA");
            booking.Flight.Confirm();

            var events = booking.As<IEventSource>().TakeEvents();

            events.Should().HaveCount(3);
            events[0].Should().BeOfType<BookingOpened>();
            events[1].Should().BeOfType<FlightRequested>();
            events[2].Should().BeOfType<FlightConfirmed>();
            
            var remainingEvents = booking.As<IEventSource>().TakeEvents();
            remainingEvents.Should().HaveCount(0);
        }
    }
}