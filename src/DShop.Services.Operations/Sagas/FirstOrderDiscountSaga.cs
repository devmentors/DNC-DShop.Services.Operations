using Chronicle;
using DShop.Common.RabbitMq;
using DShop.Services.Operations.Messages.Customers.Events;
using DShop.Services.Operations.Messages.Identity.Events;
using DShop.Services.Operations.Messages.Orders.Commands;
using DShop.Services.Operations.Messages.Orders.Events;
using System;
using System.Threading.Tasks;

namespace DShop.Services.Operations.Sagas
{
    public class FirstOrderDiscountSaga : Saga<FirstOrderDiscountSagaState>,
        ISagaStartAction<CustomerCreated>,
        ISagaAction<OrderCreated>
    {
        private const int CreationHoursLimit = 24;

        private readonly IBusPublisher _busPublisher;

        public FirstOrderDiscountSaga(IBusPublisher busPublisher)
            => _busPublisher = busPublisher;

        public override Guid ResolveId(object message, ISagaContext context)
        {
            switch (message)
            {
                case CustomerCreated cc: return cc.Id;
                case OrderCreated oc: return oc.CustomerId;
                default: return base.ResolveId(message, context);
            }
        }

        //1: Check whether customer creation hours diff fits the limit
        public Task HandleAsync(CustomerCreated message, ISagaContext context)
        {
            Data.CustomerCreatedDate = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        //2: Check whether customer creation hours diff fits the limit
        public async Task HandleAsync(OrderCreated message, ISagaContext context)
        {
            var diff = DateTime.UtcNow.Subtract(Data.CustomerCreatedDate);

            if (diff.TotalHours <= CreationHoursLimit)
            {
                await _busPublisher.SendAsync(new CreateOrderDiscount(
                    message.Id, message.CustomerId), CorrelationContext.Empty);

                Complete();
            }
            else
            {
                Reject();
            }
        }

#region Compensate

        public async Task CompensateAsync(CustomerCreated message, ISagaContext context) { }

        public async Task CompensateAsync(OrderCreated message, ISagaContext context) { }
#endregion
    }

    public class FirstOrderDiscountSagaState
    {
        public DateTime CustomerCreatedDate { get; set; }
    }
}
