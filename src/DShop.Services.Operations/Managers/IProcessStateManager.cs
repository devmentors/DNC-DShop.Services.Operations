using System;
using System.Threading.Tasks;
using DShop.Common.RabbitMq;

namespace DShop.Services.Operations.Managers
{
    public interface IProcessStateManager
    {
        Task PendingAsync(Guid id, ICorrelationContext context);
        Task CompleteAsync(Guid id, ICorrelationContext context);
        Task RejectAsync(Guid id, ICorrelationContext context, string code, string message);
    }
}