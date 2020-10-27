using System;
using System.Threading;

namespace Hitmu.Abstractions.Core.Messaging
{
    public static class MessagingConstants
    {
        public const string CommandHandlerMethodName = "HandleAsync";
        public const string QueryHandlerMethodName = "HandleAsync";
        public const string IntegrationEventHandlerName = "HandleAsync";
        public const string EventHandlerName = "HandleAsync";
        public static Type CancelationTokenType = typeof(CancellationToken);
    }
}