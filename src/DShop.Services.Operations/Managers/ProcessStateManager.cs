using System;
using System.Threading.Tasks;
using DShop.Common.RabbitMq;
using DShop.Services.Operations.Services;

namespace DShop.Services.Operations.Managers
{
    public class ProcessStateManager : IProcessStateManager
    {
        private readonly IOperationPublisher _operationPublisher;

        public ProcessStateManager(IOperationPublisher operationPublisher)
        {
            _operationPublisher = operationPublisher;
        }

        public async Task PendingAsync(Guid id, ICorrelationContext context)
            => await _operationPublisher.PendingAsync(context);

        public async Task CompleteAsync(Guid id, ICorrelationContext context)
            => await _operationPublisher.CompleteAsync(context);

        public async Task RejectAsync(Guid id, ICorrelationContext context, string code, string message)
            => await _operationPublisher.RejectAsync(context, code, message);
    }
}