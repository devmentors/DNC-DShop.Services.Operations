using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DShop.Common.Messages;
using DShop.Common.RabbitMq;

namespace DShop.Services.Operations.Managers
{
    public interface IProcessManager
    {
        Type Initiator { get; }
        IEnumerable<Type> Messages { get; }

        Task ExecuteAsync<TMessage>(TMessage message, ICorrelationContext context)
            where TMessage : IMessage;
    }

    public interface IProcessManager<TInitiator> : IProcessManager
    {
    }
}