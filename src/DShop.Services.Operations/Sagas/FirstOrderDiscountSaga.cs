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
        ISagaStartAction<SignedUp>,
        ISagaAction<CustomerCreated>,
        ISagaAction<OrderCreated>
    {
        private const int CreationHoursLimit = 24;
        
        private readonly IBusPublisher _busPublisher;

        public FirstOrderDiscountSaga(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher;
        }

        //1:Save user's creation date
        public Task HandleAsync(SignedUp message)
        {
            Data.UserCreatedDate = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        //2: Check whether user creation hours diff fits the limit
        public Task HandleAsync(CustomerCreated message)
        {
            var diff = DateTime.UtcNow.Subtract(Data.UserCreatedDate);

            if (diff.TotalHours <= CreationHoursLimit)
            {
                Data.CustomerCreatedDate = DateTime.UtcNow;
            }
            else
            {                
                Reject();
            }

            return Task.CompletedTask;
        }

        //3: Check whether customer creation hours diff fits the limit
        public async Task HandleAsync(OrderCreated message)
        {
            var diff = DateTime.UtcNow.Subtract(Data.CustomerCreatedDate);

            if (diff.TotalHours <= CreationHoursLimit)
            {
                await _busPublisher.SendAsync(new CreateOrderDiscount(
                    message.Id, message.CustomerId, message.Number), CorrelationContext.Empty);

                Complete();
            }
            else
            {
                Reject();
            }
        }

        public async Task CompensateAsync(SignedUp message) { }

        public async Task CompensateAsync(CustomerCreated message) { }

        public async Task CompensateAsync(OrderCreated message) { }
    }

    public class FirstOrderDiscountSagaState
    {
        public DateTime UserCreatedDate { get; set; }
        public DateTime CustomerCreatedDate { get; set; }
    }
}
