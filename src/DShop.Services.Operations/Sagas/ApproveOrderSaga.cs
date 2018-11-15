using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chronicle;
using DShop.Common.RabbitMq;
using DShop.Services.Operations.Messages.Orders.Commands;
using DShop.Services.Operations.Messages.Orders.Events;
using DShop.Services.Operations.Messages.Products.Commands;
using DShop.Services.Operations.Messages.Products.Events;

namespace DShop.Services.Operations.Sagas
{
    public class ApproveOrderSagaState
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public bool ProductsReserved { get; set; }
        public IDictionary<Guid, int> Products { get; set; }
    }

    public class ApproveOrderSaga : Saga<ApproveOrderSagaState>,
        ISagaStartAction<OrderCreated>,
        ISagaAction<ProductsReserved>,
        ISagaAction<ReserveProductsRejected>,
        ISagaAction<OrderApproved>,
        ISagaAction<ApproveOrderRejected>
    {
        private readonly IBusPublisher _busPublisher;

        public ApproveOrderSaga(IBusPublisher busPublisher)
            => _busPublisher = busPublisher;

        public override Guid ResolveId(object message, ISagaContext context)
        {
            switch (message)
            {
                case OrderCreated m: return m.Id;
                case ProductsReserved m: return m.OrderId;
                case ReserveProductsRejected m: return m.OrderId;
                case OrderApproved m: return m.Id;
                case ApproveOrderRejected m: return m.Id;
                default: return base.ResolveId(message, context);
            }
        }

        public async Task HandleAsync(OrderCreated message, ISagaContext context)
        {
            Data.OrderId = message.Id;
            Data.CustomerId = message.CustomerId;
            Data.Products = message.Products;
            await _busPublisher.SendAsync(new ReserveProducts(message.Id, message.Products), CorrelationContext.Empty);
        }

        public async Task CompensateAsync(OrderCreated message, ISagaContext context)
        {
            await _busPublisher.SendAsync(new CancelOrder(Data.OrderId, Data.CustomerId), CorrelationContext.Empty);
        }

        public async Task HandleAsync(ProductsReserved message, ISagaContext context)
        {
            Data.ProductsReserved = true;
            await _busPublisher.SendAsync(new ApproveOrder(Data.OrderId), CorrelationContext.Empty);
        }

        public async Task CompensateAsync(ProductsReserved message, ISagaContext context)
        {
            if (!Data.ProductsReserved)
            {
                return;
            }

            await _busPublisher.SendAsync(new ReleaseProducts(Data.OrderId, Data.Products), CorrelationContext.Empty);
        }

        public async Task HandleAsync(ReserveProductsRejected message, ISagaContext context)
        {
            Reject();
            await Task.CompletedTask;
        }

        public async Task CompensateAsync(ReserveProductsRejected message, ISagaContext context)
        {
            await Task.CompletedTask;
        }

        public async Task HandleAsync(OrderApproved message, ISagaContext context)
        {
            Complete();
            await Task.CompletedTask;
        }

        public async Task CompensateAsync(OrderApproved message, ISagaContext context)
        {
            await Task.CompletedTask;
        }

        public async Task HandleAsync(ApproveOrderRejected message, ISagaContext context)
        {
            Reject();
            await Task.CompletedTask;
        }

        public async Task CompensateAsync(ApproveOrderRejected message, ISagaContext context)
        {
            await Task.CompletedTask;
        }
    }
}