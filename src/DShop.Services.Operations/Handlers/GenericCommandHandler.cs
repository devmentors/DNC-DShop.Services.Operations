using System.Threading.Tasks;
using Chronicle;
using DShop.Common.Handlers;
using DShop.Common.Messages;
using DShop.Common.RabbitMq;
using DShop.Services.Operations.Sagas;
using DShop.Services.Operations.Services;

namespace DShop.Services.Operations.Handlers
{
    public class GenericCommandHandler<T> : ICommandHandler<T> where T : class, ICommand
    {
        private readonly ISagaCoordinator _sagaCoordinator;
        private readonly IOperationPublisher _operationPublisher;
        private readonly IOperationsStorage _operationsStorage;

        public GenericCommandHandler(ISagaCoordinator sagaCoordinator,
            IOperationPublisher operationPublisher,
            IOperationsStorage operationsStorage)
        {
            _sagaCoordinator = sagaCoordinator;
            _operationPublisher = operationPublisher;
            _operationsStorage = operationsStorage;
        }

        public async Task HandleAsync(T command, ICorrelationContext context)
        {
            if(command.IsProcessable())
            {
                var sagaContext = SagaContext.FromCorrelationContext(context);
                await _sagaCoordinator.ProcessAsync(command, sagaContext);
                return;
            }

            if (await _operationsStorage.TrySetAsync(context.Id, context.UserId,
                context.Name, OperationState.Pending, context.Resource, string.Empty))
            {
                await _operationPublisher.PendingAsync(context);
            }            
        }
    }
}