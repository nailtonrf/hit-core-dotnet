using FluentAssertions;
using Hitmu.Abstractions.Core.Messaging.Events;
using Hitmu.Abstractions.Core.State;
using System;
using System.Linq;
using Xunit;

namespace Hitmu.Abstractions.Core.Tests
{
    public class ObservableStateTests
    {
        #region Arrages
        internal class State : ObservableStateBase<Guid>
        {
            public override Guid Id { get; }
            public bool Activated { get; private set; }

            public State(Guid id)
            {
                Id = id;
                Activated = true;
            }

            public void Inactivate()
            {
                Activated = false;
                RaiseEvent(new AggregateWasInactivated(Id.ToString()));
            }
        }

        internal class AggregateWasInactivated : IEvent
        {
            public AggregateWasInactivated(string id)
            {
                Id = Guid.Parse(id);
                SourceId = id;
                CreationDate = DateTime.UtcNow;
            }

            public Guid Id { get; }
            public string SourceId { get; }
            public DateTime CreationDate { get; }
        }

        internal class IntAggregate : ObservableStateBase<int>
        {
            public IntAggregate(int id)
            {
                Id = id;
            }

            public override int Id { get; }
        }
        #endregion

        [Fact]
        public void Should_Clear_Events()
        {
            var aggregate = new State(Guid.NewGuid());
            aggregate.Inactivate();
            aggregate.Inactivate();
            aggregate.ClearEvents();
            aggregate.Events.Count().Should().Be(0);
        }

        [Fact]
        public void Should_Guid_Equals()
        {
            var guid = Guid.NewGuid();
            var aggregate1 = new State(guid);
            var aggregate2 = new State(guid);
            aggregate1.Should().Be(aggregate2);
        }

        [Fact]
        public void Should_Guid_NotEquals()
        {
            var aggregate1 = new State(Guid.NewGuid());
            var aggregate2 = new State(Guid.NewGuid());
            aggregate1.Should().NotBe(aggregate2);
        }

        [Fact]
        public void Should_Int_Equals()
        {
            var aggregate1 = new IntAggregate(1);
            var aggregate2 = new IntAggregate(1);
            aggregate1.Should().Be(aggregate2);
        }

        [Fact]
        public void Should_Int_NotEquals()
        {
            var aggregate1 = new IntAggregate(1);
            var aggregate2 = new IntAggregate(2);
            aggregate1.Should().NotBe(aggregate2);
        }

        [Fact]
        public void Should_Not_Duplicate_Events()
        {
            var aggregate = new State(Guid.NewGuid());
            aggregate.Inactivate();
            aggregate.Inactivate();
            aggregate.Events.Count().Should().Be(1);
        }
    }
}