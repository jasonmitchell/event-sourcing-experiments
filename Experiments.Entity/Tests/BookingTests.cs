using System;
using Experiments.Entity.Bookings;
using FluentAssertions;
using Xunit;

namespace Experiments.Entity.Tests
{
    public class BookingTests
    {
        [Fact]
        public void RehydratesFlight()
        {
            var e = new FlightRequested("AB123", "BHD", "LBA");
            var booking = new Booking(Guid.NewGuid());

            booking.Apply(e);

            booking.Flight.Should().NotBeNull();
            booking.Flight.Number.Should().Be("AB123");
        }

        [Fact]
        public void TracksEntityEvents()
        {
            var e = new FlightRequested("AB123", "BHD", "LBA");
            var booking = new Booking(Guid.NewGuid());

            booking.Apply(e);
            booking.ClearUncommittedEvents();
            
            booking.Flight.Confirm();

            booking.UncommittedEvents.Should().HaveCount(1).And.AllBeOfType<FlightConfirmed>();
            booking.Flight.Confirmed.Should().BeTrue();
        }
    }
}