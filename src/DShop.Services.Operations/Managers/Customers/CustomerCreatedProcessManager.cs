using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DShop.Common.RabbitMq;
using DShop.Services.Operations.Messages.Customers.Events;

namespace DShop.Services.Operations.Managers.Customers
{
    public class CustomerCreatedProcessManager : ProcessManager<CustomerCreated>
    {
        public CustomerCreatedProcessManager(IProcessStateManager processStateManager) : 
            base(new List<Type>(), processStateManager)
        {
        }

        public override async Task ExecuteAsync<TMessage>(TMessage message, ICorrelationContext context)
        {
            await CompleteAsync(Guid.Empty, context);
        }
    }
}