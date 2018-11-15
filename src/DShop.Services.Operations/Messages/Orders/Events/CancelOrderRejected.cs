using System;
using DShop.Common.Messages;
using Newtonsoft.Json;

namespace DShop.Services.Operations.Messages.Orders.Events
{
    [MessageNamespace("orders")]
    public class CancelOrderRejected : IRejectedEvent
    {
        public Guid Id { get; }
        public Guid CustomerId { get; }
        public string Reason { get; }
        public string Code { get; }

        [JsonConstructor]
        public CancelOrderRejected(Guid id, Guid customerId, string reason, string code)
        {
            Id = id;
            CustomerId = customerId;
            Reason = reason;
            Code = code;
        }
    }
}