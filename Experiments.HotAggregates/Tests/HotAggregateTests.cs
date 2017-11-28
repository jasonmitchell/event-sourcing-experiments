using System;
using System.Collections.Generic;
using System.Linq;
using Experiments.HotAggregates.Sales;
using FluentAssertions;
using Xunit;

namespace Experiments.HotAggregates.Tests
{
    public class HotAggregateTests
    {
        private readonly List<(string ticketId, string stream, object[] events)> streams;

        public HotAggregateTests()
        {
            streams = new List<(string ticketId, string stream, object[] events)>();
            for (var i = 0; i < 3; i++)
            {
                var ticketId = Guid.NewGuid().ToString();
                var streamId = $"TicketSale-{ticketId}";
                var events = new object[]
                {
                    new TicketSaleScheduled(ticketId, 1000),
                    new TicketReservationConfirmed(ticketId, Guid.NewGuid().ToString(), 3),
                    new TicketReservationConfirmed(ticketId, Guid.NewGuid().ToString(), 2),
                    new TicketReservationConfirmed(ticketId, Guid.NewGuid().ToString(), 6),
                    new TicketReservationConfirmed(ticketId, Guid.NewGuid().ToString(), 4)
                };

                streams.Add((ticketId, streamId, events));
            }
        }

        [Fact]
        public void DoesNotLoadAggregateMultipleTimesForSameStream()
        {
            var readCount = 0;
            var repository = new AggregateRepository(stream =>
            {
                readCount++;
                return streams.Single(x => x.stream == stream).events;
            }, (stream, events) =>
            {
                // Nothing to do here
            });

            var a = repository.Load<TicketSale>(streams[0].ticketId);
            a.Should().NotBeNull();
            readCount.Should().Be(1);

            var b = repository.Load<TicketSale>(streams[0].ticketId);
            b.Should().NotBeNull().And.BeSameAs(a);
            readCount.Should().Be(1);

            var c = repository.Load<TicketSale>(streams[0].ticketId);
            c.Should().NotBeNull().And.BeSameAs(a);
            readCount.Should().Be(1);
        }

        [Fact]
        public void ReturnsDifferentInstancesOfAggregateForDifferentStreams()
        {
            var readCount = 0;
            var repository = new AggregateRepository(stream =>
            {
                readCount++;
                return streams.Single(x => x.stream == stream).events;
            }, (stream, events) =>
            {
                // Nothing to do here
            });

            var a = repository.Load<TicketSale>(streams[0].ticketId);
            a.Should().NotBeNull();
            readCount.Should().Be(1);

            var b = repository.Load<TicketSale>(streams[1].ticketId);
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

        [Fact]
        public void RemovesOldestAggregateFromMemoryWhenMaxCountReached()
        {
            var readCount = 0;
            var repository = new AggregateRepository(stream =>
            {
                readCount++;
                return streams.Single(x => x.stream == stream).events;
            }, (stream, events) =>
            {
                // Nothing to do here
            }, 2);

            var a = repository.Load<TicketSale>(streams[0].ticketId);
            a.Should().NotBeNull();
            readCount.Should().Be(1);

            var b = repository.Load<TicketSale>(streams[1].ticketId);
            b.Should().NotBeNull().And.NotBeSameAs(a);
            readCount.Should().Be(2);

            var c = repository.Load<TicketSale>(streams[2].ticketId);
            c.Should().NotBeNull().And.NotBeSameAs(a);
            readCount.Should().Be(3);

            // Reading 'A' again should force another read because it should have dropped off
            var a_again = repository.Load<TicketSale>(streams[0].ticketId);
            a_again.Should().NotBeNull().And.NotBeSameAs(a);
            readCount.Should().Be(4);
        }

        [Fact]
        public void DoesNotRemoveAggregateWhenMaxCountLimitReachedAndReadingCachedAggregate()
        {
            var readCount = 0;
            var repository = new AggregateRepository(stream =>
            {
                readCount++;
                return streams.Single(x => x.stream == stream).events;
            }, (stream, events) =>
            {
                // Nothing to do here
            }, 2);

            var a = repository.Load<TicketSale>(streams[0].ticketId);
            a.Should().NotBeNull();
            readCount.Should().Be(1);

            var b = repository.Load<TicketSale>(streams[1].ticketId);
            b.Should().NotBeNull().And.NotBeSameAs(a);
            readCount.Should().Be(2);

            // Reading 'A' again should not cause the original to drop out of memory
            var a_again = repository.Load<TicketSale>(streams[0].ticketId);
            a_again.Should().NotBeNull().And.BeSameAs(a);
            readCount.Should().Be(2);
        }
    }
}