using System.Threading.Tasks;
using DShop.Common.Handlers;
using DShop.Common.RabbitMq;
using DShop.Messages.Events;
using DShop.Messages.Events.Customers;
using DShop.Services.Operations.Services;

namespace DShop.Services.Operations.Handlers
{
    public class GenericEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IEvent
    {
        private readonly IOperationsService _operationsService;

        public GenericEventHandler(IOperationsService operationsService)
        {
            _operationsService = operationsService;
        }

        public async Task HandleAsync(TEvent @event, ICorrelationContext context)
        {
            if(@event is IRejectedEvent rejectedEvent)
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
        }

        private async Task RejectAsync(IRejectedEvent @event, ICorrelationContext context)
        {
            await _operationsService.RejectAsync(context.Id, @event.Code, @event.Reason);
        }
    }
}