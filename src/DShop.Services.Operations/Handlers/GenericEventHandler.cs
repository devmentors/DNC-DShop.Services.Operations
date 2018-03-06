using System.Threading.Tasks;
using DShop.Common.Handlers;
using DShop.Common.RabbitMq;
using DShop.Messages.Events;
using DShop.Messages.Events.Customers;
using DShop.Services.Operations.Services;

namespace DShop.Services.Operations.Handlers
{
    public class GenericEventHandler : IEventHandler<CustomerCreated>,
        IEventHandler<CreateCustomerRejected>
    {
        private readonly IOperationsService _operationsService;

        public GenericEventHandler(IOperationsService operationsService)
        {
            _operationsService = operationsService;
        }

        public async Task HandleAsync(CustomerCreated @event, ICorrelationContext context)
            => await CompleteAsync(context);

        public async Task HandleAsync(CreateCustomerRejected @event, ICorrelationContext context)
            => await RejectAsync(@event, context);

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