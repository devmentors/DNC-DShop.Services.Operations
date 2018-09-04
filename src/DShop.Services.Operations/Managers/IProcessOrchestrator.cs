using System.Threading.Tasks;
using DShop.Common.Messages;
using DShop.Common.RabbitMq;

namespace DShop.Services.Operations.Managers
{
    public interface IProcessOrchestrator
    {
        bool IsProcessable<TMessage>() where TMessage : IMessage;

        Task ExecuteAsync<TMessage>(TMessage message, ICorrelationContext context)
            where TMessage : IMessage;
    }
}