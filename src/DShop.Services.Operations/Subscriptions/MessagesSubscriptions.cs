using DShop.Common.Messages;
using DShop.Common.RabbitMq;
using DShop.Services.Operations.Messages.Events;
using System;
using System.Linq;
using System.Reflection;

namespace DShop.Services.Operations.Subscriptions
{
    public static class MessagesSubscriptions
    {
        private static Assembly _messagesAssembly => typeof(MessagesSubscriptions).Assembly;

        public static IBusSubscriber SubscribeAllCommands(this IBusSubscriber subscriber)
            => subscriber.SubscribeAllMessages<ICommand>(nameof(IBusSubscriber.SubscribeCommand));

        public static IBusSubscriber SubscribeAllEvents(this IBusSubscriber subscriber)
            => subscriber.SubscribeAllMessages<IEvent>(nameof(IBusSubscriber.SubscribeEvent));

        private static IBusSubscriber SubscribeAllMessages<TMessage>
            (this IBusSubscriber subscriber, string subscribeMethod)
        {            
            var messageTypes = _messagesAssembly
                .GetTypes()
                .Where(t => t.IsClass && typeof(TMessage).IsAssignableFrom(t))
                .Where(t => t != typeof(OperationUpdated))
                .ToList();

            messageTypes.ForEach(mt => subscriber.GetType()
                    .GetMethod(subscribeMethod)
                    .MakeGenericMethod(mt)
                    .Invoke(subscriber, new object[] { null }));

            return subscriber;
        }
    }
}