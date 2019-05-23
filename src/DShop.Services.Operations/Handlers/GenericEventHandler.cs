using System.Threading.Tasks;
using Chronicle;
using DShop.Common.Handlers;
using DShop.Common.Messages;
using DShop.Common.RabbitMq;
using DShop.Services.Operations.Sagas;
using DShop.Services.Operations.Services;
using SagaContext = DShop.Services.Operations.Sagas.SagaContext;

namespace DShop.Services.Operations.Handlers
{
    public class GenericEventHandler<T> : IEventHandler<T> where T : class, IEvent
    {
        private readonly ISagaCoordinator _sagaCoordinator;
        private readonly IOperationPublisher _operationPublisher;
        private readonly IOperationsStorage _operationsStorage;

        public GenericEventHandler(ISagaCoordinator sagaCoordinator,
            IOperationPublisher operationPublisher,
            IOperationsStorage operationsStorage)
        {
            _sagaCoordinator = sagaCoordinator;
            _operationPublisher = operationPublisher;
            _operationsStorage = operationsStorage;
        }

        public async Task HandleAsync(T @event, ICorrelationContext context)
        {
            if (@event.BelongsToSaga())
            {
                var sagaContext = SagaContext.FromCorrelationContext(context);
                await _sagaCoordinator.ProcessAsync(@event, sagaContext);
            }

            switch (@event)
            {
                case IRejectedEvent rejectedEvent:
                    await _operationsStorage.SetAsync(context.Id, context.UserId,
                        context.Name, OperationState.Rejected, context.Resource,
                        rejectedEvent.Code, rejectedEvent.Reason);
                    await _operationPublisher.RejectAsync(context,
                        rejectedEvent.Code, rejectedEvent.Reason);
                    return;
                case IEvent _:
                    await _operationsStorage.SetAsync(context.Id, context.UserId,
                        context.Name, OperationState.Completed, context.Resource);
                    await _operationPublisher.CompleteAsync(context);
                    return;
            }
        }
    }
}