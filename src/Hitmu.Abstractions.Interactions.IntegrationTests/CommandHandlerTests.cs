using FluentAssertions;
using Hitmu.Abstractions.Core.Initializer;
using Hitmu.Abstractions.Core.Messaging.Commands;
using Hitmu.Abstractions.Core.Messaging.Events;
using Hitmu.Abstractions.Core.Results;
using Hitmu.Abstractions.Interactions.IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Hitmu.Abstractions.Interactions.IntegrationTests
{
    public class CommandHandlerTests
    {
        [Fact]
        public void Should_Run_CommandHandler()
        {
            using var applicationContext = ApplicationContextExtensions.CreateApplicationContext();
            applicationContext.Load(new FirstComponent());
            applicationContext.InitializeMediators();
            applicationContext.Start();
            using (var scope = applicationContext.BeginScope())
            {
                var iteratorContext = scope.GetService<IIteratorContext>();
                var result = iteratorContext
                    .ProcessAsync(new Command("1"), new CancellationToken())
                    .GetAwaiter()
                    .GetResult();
                result.Should().NotBeNull();
                result.IsValid.Should().BeTrue();
                result.Value.Name.Should().Be("command_result");
            }
        }

        [Fact]
        public void Should_Run_CommandHandler_With_Events()
        {
            using var applicationContext = ApplicationContextExtensions.CreateApplicationContext();
            applicationContext.Load(new FirstComponent());
            applicationContext.InitializeMediators();
            applicationContext.Start();
            using (var scope = applicationContext.BeginScope())
            {
                var commandSender = scope.GetService<IIteratorContext>();
                var result = commandSender.ProcessAsync(new Command("1"), new CancellationToken()).GetAwaiter()
                    .GetResult();
                result.Should().NotBeNull();
                result.IsValid.Should().BeTrue();
                result.Value.Name.Should().Be("command_result");
            }
        }
    }

    #region Setups

    public sealed class FirstComponent : IComponentInitializer
    {
        public string Name => "FirstComponent";
        public void Initialize(IServiceCollection services)
        {
            services
                .AddScoped<ICommandHandler<Command, CommandResult>, CommandHandler>()
                .AddScoped<IEventHandler<EventFromCommand>, EventFromCommandHandler>();
        }
    }

    public class CommandResult : ICommandResult
    {
        public CommandResult(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public class Command : ICommand<CommandResult>
    {
        public Command(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }

    public class CommandHandler : ICommandHandler<Command, CommandResult>
    {
        private readonly IEventIterator _eventMediator;

        public CommandHandler(IEventIterator eventMediator)
        {
            _eventMediator = eventMediator;
        }

        public async Task<Result<CommandResult>> HandleAsync(Command command, CancellationToken cancellationToken)
        {
            await _eventMediator.Publish(new EventFromCommand());
            return Result<CommandResult>.From(new CommandResult("command_result"));
        }
    }

    public class EventFromCommand : IEvent
    {
        public EventFromCommand()
        {
            Id = Guid.NewGuid();
            SourceId = Guid.NewGuid().ToString();
            CreationDate = DateTime.UtcNow;
            Owner = "MicroservicoOrigem";
        }

        public string Owner { get; }
        public Guid Id { get; }
        public string SourceId { get; }
        public DateTime CreationDate { get; }
    }

    public class EventFromCommandHandler : IEventHandler<EventFromCommand>
    {
        public Task<Result<bool>> HandleAsync(EventFromCommand @event, CancellationToken cancellationToken)
        {
            return Task.FromResult(Result<bool>.From(true));
        }
    }

    #endregion
}