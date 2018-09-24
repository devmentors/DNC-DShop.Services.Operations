using DShop.Common.Messages;
using Newtonsoft.Json;
using System;

namespace DShop.Services.Operations.Messages.Orders.Commands
{
    [MessageNamespace("orders")]
    public class CreateOrderDiscount : ICommand
    {
        public Guid Id { get; }
        public Guid CustomerId { get; }
        public long Number { get; }

        [JsonConstructor]
        public CreateOrderDiscount(Guid id, Guid customerId, long number)
        {
            Id = id;
            CustomerId = customerId;
            Number = number;
        }
    }
}
