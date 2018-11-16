using System;
using DShop.Common.Messages;
using Newtonsoft.Json;

namespace DShop.Services.Operations.Messages.Orders.Commands
{
    public class RevokeOrder : ICommand
    {
        public Guid Id { get; }
        public Guid CustomerId { get; }

        [JsonConstructor]
        public RevokeOrder(Guid id, Guid customerId)
        {
            Id = id;
            CustomerId = customerId;
        }
    }
}