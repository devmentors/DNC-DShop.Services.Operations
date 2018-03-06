using DShop.Common.RabbitMq;
using DShop.Messages.Commands.Customers;
using DShop.Messages.Events.Customers;

namespace DShop.Services.Operations.Subscriptions
{
    public static class CustomerSubscriptions
    {
        public static IBusSubscriber SubscribeCustomers(this IBusSubscriber subscriber)
            => subscriber.SubscribeCommands()
                         .SubscribeEvents();

        private static IBusSubscriber SubscribeCommands(this IBusSubscriber subscriber)
            => subscriber.SubscribeCommand<CreateCustomer>();

        private static IBusSubscriber SubscribeEvents(this IBusSubscriber subscriber)
            => subscriber.SubscribeEvent<CustomerCreated>()
                .SubscribeEvent<CreateCustomerRejected>();
    }
}