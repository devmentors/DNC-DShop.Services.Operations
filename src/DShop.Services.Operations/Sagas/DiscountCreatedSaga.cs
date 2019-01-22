using System.Threading.Tasks;
using Chronicle;
using DShop.Common.RabbitMq;
using DShop.Services.Operations.Messages.Discounts.Events;
using DShop.Services.Operations.Messages.Notifications.Commands;

namespace DShop.Services.Operations.Sagas
{
    public class DiscountCreatedSaga : Saga,
        ISagaStartAction<DiscountCreated>
    {
        private readonly IBusPublisher _busPublisher;

        public DiscountCreatedSaga(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher;
        }
        
        public Task HandleAsync(DiscountCreated message, ISagaContext context)
        {
            return _busPublisher.SendAsync(new SendEmailNotification("user1@dshop.com",
                    "Discount", $"New discount: {message.Code}"),
                CorrelationContext.Empty);
        }

        public Task CompensateAsync(DiscountCreated message, ISagaContext context)
        {
            return Task.CompletedTask;
        }
    }

    public class State
    {
        public string Code { get; set; }
    }
}