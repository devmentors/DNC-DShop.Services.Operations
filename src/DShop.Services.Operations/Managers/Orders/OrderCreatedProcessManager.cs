using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DShop.Common.Messages;
using DShop.Common.RabbitMq;
using DShop.Services.Operations.Messages.Orders.Events;

namespace DShop.Services.Operations.Managers.Orders
{
    public class OrderCreatedProcessManager : ProcessManager<OrderCreated>
    {
        public OrderCreatedProcessManager(IProcessStateManager processStateManager) :
            base(new List<Type>(), processStateManager)
        {
        }

        public override async Task ExecuteAsync<TMessage>(TMessage message,
            ICorrelationContext context)
        {
            await PendingAsync(Guid.Empty, context);
            await Task.Delay(5000);
            await CompleteAsync(Guid.Empty, context);
        }
    }
}