using System.Threading.Tasks;
using DShop.Common.Handlers;
using DShop.Common.Messages;
using DShop.Common.RabbitMq;
using DShop.Services.Operations.Messages.Events;
using DShop.Services.Operations.Services;

namespace DShop.Services.Operations.Handlers
{
    public class GenericEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IEvent
    {
        private readonly IOperationsService _operationsService;
        private readonly IBusPublisher _busPublisher;

        public GenericEventHandler(IBusPublisher busPublisher,
            IOperationsService operationsService)
        {
            _operationsService = operationsService;
            _busPublisher = busPublisher;
        }

        public async Task HandleAsync(TEvent @event, ICorrelationContext context)
        {
            if (@event is IRejectedEvent rejectedEvent)
            {
                await RejectAsync(rejectedEvent, context);
            }
            else
            {
                await CompleteAsync(context);
            }
        }

        private async Task CompleteAsync(ICorrelationContext context)
        {
            await _operationsService.CompleteAsync(context.Id);
            await PublishOperationUpdatedAsync(context);
        }

        private async Task RejectAsync(IRejectedEvent @event, ICorrelationContext context)
        {
            await _operationsService.RejectAsync(context.Id, @event.Code, @event.Reason);
            await PublishOperationUpdatedAsync(context);
        }

        private async Task PublishOperationUpdatedAsync(ICorrelationContext context)
        {
            var operation = await _operationsService.GetAsync(context.Id);
            await _busPublisher.PublishAsync(new OperationUpdated(context.Id,
                context.UserId, context.Name, context.Origin, context.Resource,
                    operation.State, operation.Code, operation.Message), context);            
        }
    }
}