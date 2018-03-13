using System.Threading.Tasks;
using DShop.Common.Handlers;
using DShop.Common.RabbitMq;
using DShop.Messages.Commands;
using DShop.Messages.Commands.Customers;
using DShop.Messages.Events.Operations;
using DShop.Services.Operations.Services;

namespace DShop.Services.Operations.Handlers
{
    public class GenericCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly IOperationsService _operationsService;
        private readonly IBusPublisher _busPublisher;

        public GenericCommandHandler(IBusPublisher busPublisher,
            IOperationsService operationsService)
        {
            _operationsService = operationsService;
            _busPublisher = busPublisher;
        }

        public async Task HandleAsync(TCommand command, ICorrelationContext context)
            => await HandleAsync(context);

        private async Task HandleAsync(ICorrelationContext context)
        {
            await _operationsService.CreateAsync(context.Id, context.Name, context.UserId,
                context.Origin, context.Resource, context.CreatedAt);
            var operation = await _operationsService.GetAsync(context.Id);
            await _busPublisher.PublishEventAsync(new OperationUpdated(context.Id,
                context.UserId, context.Name, context.Origin, context.Resource,
                    operation.State, operation.Code, operation.Message), context);
        }
    }
}