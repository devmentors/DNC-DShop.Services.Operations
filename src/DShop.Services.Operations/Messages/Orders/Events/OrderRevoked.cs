using System;
using DShop.Common.Messages;
using Newtonsoft.Json;

namespace DShop.Services.Operations.Messages.Orders.Events
{
    public class OrderRevoked : IEvent
    {
        public Guid Id { get; }
        public Guid CustomerId { get; }

        [JsonConstructor]
        public OrderRevoked(Guid id, Guid customerId)
        {
            Id = id;
            CustomerId = customerId;
        }
    }
}