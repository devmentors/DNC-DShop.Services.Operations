using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DShop.Common.Messages;
using DShop.Common.RabbitMq;

namespace DShop.Services.Operations.Managers
{
    public abstract class ProcessManager<TInitiator> : IProcessManager<TInitiator>
    {
        private readonly IProcessStateManager _processStateManager;
        public Type Initiator => typeof(TInitiator);
        public IEnumerable<Type> Messages { get; }

        protected ProcessManager(IEnumerable<Type> messages,
            IProcessStateManager processStateManager)
        {
            Messages = messages;
            _processStateManager = processStateManager;
        }

        public abstract Task ExecuteAsync<TMessage>(TMessage message,
            ICorrelationContext context) where TMessage : IMessage;

        protected async Task PendingAsync(Guid id, ICorrelationContext context)
            => await _processStateManager.PendingAsync(id, context);

        protected async Task CompleteAsync(Guid id, ICorrelationContext context)
            => await _processStateManager.CompleteAsync(id, context);

        protected async Task RejectAsync(Guid id, ICorrelationContext context, string code, string message)
            => await _processStateManager.RejectAsync(id, context, code, message);
    }
}