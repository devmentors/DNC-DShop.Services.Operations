using Chronicle;
using DShop.Common.RabbitMq;
using System;

namespace DShop.Services.Operations.Sagas
{
    public class SagaContext : ISagaContext
    {
        public Guid CorrelationId { get; }

        public string Originator { get; }

        private SagaContext(Guid correlationId, string originator)
            => (CorrelationId, Originator) = (correlationId, originator);

        public static ISagaContext Empty
            => new SagaContext(Guid.Empty, string.Empty);

        public static ISagaContext FromCorrelationContext(ICorrelationContext context)
            => new SagaContext(context.Id, context.Resource);
    }
}
