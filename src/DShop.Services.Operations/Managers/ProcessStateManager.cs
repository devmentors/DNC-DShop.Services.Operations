using System;
using System.Threading.Tasks;
using DShop.Common.RabbitMq;
using DShop.Services.Operations.Services;

namespace DShop.Services.Operations.Managers
{
    public class ProcessStateManager : IProcessStateManager
    {
        private readonly IOperationPublisher _operationPublisher;
        private readonly IOperationsStorage _operationsStorage;

        public ProcessStateManager(IOperationPublisher operationPublisher,
            IOperationsStorage operationsStorage)
        {
            _operationPublisher = operationPublisher;
            _operationsStorage = operationsStorage;
        }

        public async Task PendingAsync(Guid id, ICorrelationContext context)
        {
            if (await _operationsStorage.TrySetAsync(context.Id, context.UserId,
                context.Name, OperationState.Pending, context.Resource))
            {
                await _operationPublisher.PendingAsync(context);
            }
        }

        public async Task CompleteAsync(Guid id, ICorrelationContext context)
        {
            if (await _operationsStorage.TrySetAsync(context.Id, context.UserId,
                context.Name, OperationState.Completed, context.Resource))
            {
                await _operationPublisher.CompleteAsync(context);
            }
        }

        public async Task RejectAsync(Guid id, ICorrelationContext context, string code, string message)
        {
            if (await _operationsStorage.TrySetAsync(context.Id, context.UserId,
                context.Name, OperationState.Rejected, context.Resource, code, message))
            {
                await _operationPublisher.RejectAsync(context, code, message);
            }
        }
    }
}