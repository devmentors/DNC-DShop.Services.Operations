using System.Threading.Tasks;
using DShop.Common.Handlers;
using DShop.Common.Messages;
using DShop.Common.RabbitMq;
using DShop.Services.Operations.Managers;
using DShop.Services.Operations.Services;

namespace DShop.Services.Operations.Handlers
{
    public class GenericEventHandler<T> : IEventHandler<T> where T : IEvent
    {
        private readonly IProcessOrchestrator _processOrchestrator;
        private readonly IOperationPublisher _operationPublisher;
        private readonly IOperationsStorage _operationsStorage;

        public GenericEventHandler(IProcessOrchestrator processOrchestrator,
            IOperationPublisher operationPublisher,
            IOperationsStorage operationsStorage)
        {
            _processOrchestrator = processOrchestrator;
            _operationPublisher = operationPublisher;
            _operationsStorage = operationsStorage;
        }

        public async Task HandleAsync(T @event, ICorrelationContext context)
        {
            if (_processOrchestrator.IsProcessable<T>())
            {
                await _processOrchestrator.ExecuteAsync(@event, context);

                return;
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