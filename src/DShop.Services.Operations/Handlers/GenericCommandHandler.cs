using System.Threading.Tasks;
using DShop.Common.Handlers;
using DShop.Common.Messages;
using DShop.Common.RabbitMq;
using DShop.Services.Operations.Managers;
using DShop.Services.Operations.Services;

namespace DShop.Services.Operations.Handlers
{
    public class GenericCommandHandler<T> : ICommandHandler<T> where T : ICommand
    {
        private readonly IProcessOrchestrator _processOrchestrator;
        private readonly IOperationPublisher _operationPublisher;

        public GenericCommandHandler(IProcessOrchestrator processOrchestrator,
            IOperationPublisher operationPublisher)
        {
            _processOrchestrator = processOrchestrator;
            _operationPublisher = operationPublisher;
        }

        public async Task HandleAsync(T command, ICorrelationContext context)
        {
            if (_processOrchestrator.IsProcessable<T>())
            {
                await _processOrchestrator.ExecuteAsync(command, context);

                return;
            }
        }
    }
}