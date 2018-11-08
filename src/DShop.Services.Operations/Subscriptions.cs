using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DShop.Common.Messages;
using DShop.Common.RabbitMq;
using DShop.Services.Operations.Messages.Operations.Events;

namespace DShop.Services.Operations
{
    public static class Subscriptions
    {
        private static readonly Assembly MessagesAssembly = typeof(Subscriptions).Assembly;

        private static readonly ISet<Type> ExcludedMessages = new HashSet<Type>(new[]
        {
            typeof(OperationPending),
            typeof(OperationCompleted),
            typeof(OperationRejected)
        });

        public static IBusSubscriber SubscribeAllMessages(this IBusSubscriber subscriber)
            => subscriber.SubscribeAllCommands().SubscribeAllEvents();

        private static IBusSubscriber SubscribeAllCommands(this IBusSubscriber subscriber)
            => subscriber.SubscribeAllMessages<ICommand>(nameof(IBusSubscriber.SubscribeCommand));

        private static IBusSubscriber SubscribeAllEvents(this IBusSubscriber subscriber)
            => subscriber.SubscribeAllMessages<IEvent>(nameof(IBusSubscriber.SubscribeEvent));

        private static IBusSubscriber SubscribeAllMessages<TMessage>
            (this IBusSubscriber subscriber, string subscribeMethod)
        {
            var messageTypes = MessagesAssembly
                .GetTypes()
                .Where(t => t.IsClass && typeof(TMessage).IsAssignableFrom(t))
                .Where(t => !ExcludedMessages.Contains(t))
                .ToList();

            messageTypes.ForEach(mt => subscriber.GetType()
                .GetMethod(subscribeMethod)
                .MakeGenericMethod(mt)
                .Invoke(subscriber,
                    new object[] {mt.GetCustomAttribute<MessageNamespaceAttribute>()?.Namespace, null, null}));

            return subscriber;
        }
    }
}