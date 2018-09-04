using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DShop.Common.Messages;
using DShop.Common.RabbitMq;

namespace DShop.Services.Operations.Managers
{
    public class ProcessOrchestrator : IProcessOrchestrator
    {
        private readonly ISet<IProcessManager> _processManagers;
        private readonly ISet<Type> _initiators;
        private readonly ISet<Type> _messages;

        public ProcessOrchestrator(IEnumerable<IProcessManager> processManagers)
        {
            _processManagers = new HashSet<IProcessManager>(processManagers);
            _initiators = new HashSet<Type>(_processManagers.Select(p => p.Initiator));
            _messages = new HashSet<Type>(_processManagers.SelectMany(p => p.Messages));
        }

        public bool IsProcessable<T>() where T : IMessage
            => _initiators.Contains(typeof(T)) || _messages.Contains(typeof(T));

        public async Task ExecuteAsync<TMessage>(TMessage message,
            ICorrelationContext context)
            where TMessage : IMessage
        {
            ValidateMessage<TMessage>();

            var processManager = _initiators.Contains(typeof(TMessage))
                ? _processManagers.First(p => p.Initiator == typeof(TMessage))
                : _processManagers.First(p => p.Messages.Contains(typeof(TMessage)));

            await processManager.ExecuteAsync(message, context);
        }

        private void ValidateMessage<T>() where T : IMessage
        {
            if (!IsProcessable<T>())
            {
                throw new InvalidOperationException($"Message: {typeof(T).Name} " +
                                                    $"is not a part of process.");
            }
        }
    }
}