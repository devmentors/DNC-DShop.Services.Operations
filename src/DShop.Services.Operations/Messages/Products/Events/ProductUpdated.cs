using System;
using DShop.Common.Messages;
using Newtonsoft.Json;

namespace DShop.Services.Operations.Messages.Products.Events
{
    [MessageNamespace("products")]
    public class ProductUpdated : IEvent
    {
        public Guid Id { get; }

        [JsonConstructor]
        public ProductUpdated(Guid id)
        {
            Id = id;
        }
    }
}
