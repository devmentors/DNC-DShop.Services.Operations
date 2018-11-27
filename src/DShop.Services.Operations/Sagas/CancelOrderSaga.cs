using System.Threading.Tasks;
using Chronicle;
using DShop.Common.RabbitMq;
using DShop.Services.Operations.Messages.Orders.Events;
using DShop.Services.Operations.Messages.Products.Commands;
using DShop.Services.Operations.Messages.Products.Events;

namespace DShop.Services.Operations.Sagas
{
    public class CancelOrderSaga : Saga,
        ISagaStartAction<OrderCanceled>,
        ISagaAction<CancelOrderRejected>,
        ISagaAction<ProductsReleased>,
        ISagaAction<ReleaseProductsRejected>
    {
        private readonly IBusPublisher _busPublisher;

        public CancelOrderSaga(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher;
        }

        public async Task HandleAsync(OrderCanceled message, ISagaContext context)
        {
            await _busPublisher.SendAsync(new ReleaseProducts(message.Id, message.Products),
                CorrelationContext.FromId(context.CorrelationId));
        }

        public async Task CompensateAsync(OrderCanceled message, ISagaContext context)
        {
            await Task.CompletedTask;
        }

        //Edge case
        public async Task HandleAsync(CancelOrderRejected message, ISagaContext context)
        {
            Reject();
            await Task.CompletedTask;
        }

        public async Task CompensateAsync(CancelOrderRejected message, ISagaContext context)
        {
            await Task.CompletedTask;
        }

        public async Task HandleAsync(ProductsReleased message, ISagaContext context)
        {
            Complete();
            await Task.CompletedTask;
        }

        public async Task CompensateAsync(ProductsReleased message, ISagaContext context)
        {
            await Task.CompletedTask;
        }

        //Edge case
        public async Task HandleAsync(ReleaseProductsRejected message, ISagaContext context)
        {
            Reject();
            await Task.CompletedTask;
        }

        public async Task CompensateAsync(ReleaseProductsRejected message, ISagaContext context)
        {
            await Task.CompletedTask;
        }
    }
}