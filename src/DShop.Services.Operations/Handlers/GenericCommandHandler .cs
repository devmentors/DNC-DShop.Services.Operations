using System.Threading.Tasks;
using DShop.Common.Handlers;
using DShop.Common.RabbitMq;
using DShop.Messages.Commands;
using DShop.Messages.Commands.Customers;
using DShop.Services.Operations.Services;

namespace DShop.Services.Operations.Handlers
{
    public class GenericCommandHandler : ICommandHandler<CreateCustomer>
    {
        private readonly IOperationsService _operationsService;

        public GenericCommandHandler(IOperationsService operationsService)
        {
            _operationsService = operationsService;
        }

        public async Task HandleAsync(CreateCustomer command, ICorrelationContext context)
            => await HandleAsync(context);

        private async Task HandleAsync(ICorrelationContext context)
        {
            await _operationsService.CreateAsync(context.Id, context.Name, context.UserId,
                context.Origin, context.Resource, context.CreatedAt);
        }
    }
}