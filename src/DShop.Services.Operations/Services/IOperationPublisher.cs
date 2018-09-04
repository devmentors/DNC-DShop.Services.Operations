using System.Threading.Tasks;
using DShop.Common.RabbitMq;

namespace DShop.Services.Operations.Services
{
    public interface IOperationPublisher
    {
        Task PendingAsync(ICorrelationContext context);
        Task CompleteAsync(ICorrelationContext context);
        Task RejectAsync(ICorrelationContext context, string code, string message);
    }
}