using System;
using Experiments.HotAggregates.Sales;
using FluentAssertions;
using Xunit;

namespace Experiments.HotAggregates.Tests
{
    public class HotAggregateTests
    {
        [Fact]
        public void DoesNotLoadAggregateMultipleTimesForSameStream()
        {
            var readCount = 0;
            var ticketId = Guid.NewGuid().ToString();
            var repository = new AggregateRepository(stream =>
            {
                readCount++;

                return new object[]
                {
                    new TicketSaleScheduled(ticketId, 1000),
                    new TicketReservationConfirmed(ticketId, Guid.NewGuid().ToString(), 3),
                    new TicketReservationConfirmed(ticketId, Guid.NewGuid().ToString(), 2),
                    new TicketReservationConfirmed(ticketId, Guid.NewGuid().ToString(), 6),
                    new TicketReservationConfirmed(ticketId, Guid.NewGuid().ToString(), 4)
                };
            }, (stream, events) =>
            {
                // Nothing to do here
            });

            var a = repository.Load<TicketSale>(ticketId);
            a.Should().NotBeNull();
            readCount.Should().Be(1);

            var b = repository.Load<TicketSale>(ticketId);
            b.Should().NotBeNull().And.BeSameAs(a);
            readCount.Should().Be(1);

            var c = repository.Load<TicketSale>(ticketId);
            c.Should().NotBeNull().And.BeSameAs(a);
            readCount.Should().Be(1);
        }

        [Fact]
        public void ReturnsDifferentInstancesOfAggregateForDifferentStreams()
        {
            var readCount = 0;
            var ticketAId = Guid.NewGuid().ToString();
            var ticketBId = Guid.NewGuid().ToString();
            var repository = new AggregateRepository(stream =>
            {
                readCount++;

                var ticketId = stream.Contains(ticketAId) ? ticketAId : ticketBId;
                return new object[]
                {
                    new TicketSaleScheduled(ticketId, 1000),
                    new TicketReservationConfirmed(ticketId, Guid.NewGuid().ToString(), 3),
                    new TicketReservationConfirmed(ticketId, Guid.NewGuid().ToString(), 2),
                    new TicketReservationConfirmed(ticketId, Guid.NewGuid().ToString(), 6),
                    new TicketReservationConfirmed(ticketId, Guid.NewGuid().ToString(), 4)
                };
            }, (stream, events) =>
            {
                // Nothing to do here
            });

            var a = repository.Load<TicketSale>(ticketAId);
            a.Should().NotBeNull();
            readCount.Should().Be(1);

            var b = repository.Load<TicketSale>(ticketBId);
            b.Should().NotBeNull().And.NotBeSameAs(a);
            readCount.Should().Be(2);
        }

        [Fact(Skip = "TODO")]
        public void ExtendsLifespanWhenAggregateIsReturned()
        {

        }

        [Fact(Skip = "TODO")]
        public void ExtendsLifespanWhenAggregateIsSaved()
        {

        }

        [Fact(Skip = "TODO")]
        public void RemovesAggregateFromMemoryWhenLifespanExceeded()
        {

        }

        [Fact(Skip = "TODO")]
        public void RemovesOldestAggregateFromMemoryWhenMaxCountReached()
        {
            
        }
    }
}