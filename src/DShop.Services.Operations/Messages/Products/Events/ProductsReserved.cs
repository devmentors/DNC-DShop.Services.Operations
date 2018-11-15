using System;
using System.Collections.Generic;
using DShop.Common.Messages;
using Newtonsoft.Json;

namespace DShop.Services.Operations.Messages.Products.Events
{
    [MessageNamespace("products")]
    public class ProductsReserved : IEvent
    {
        public Guid OrderId { get; set; }
        public IDictionary<Guid, int> Products { get; }

        [JsonConstructor]
        public ProductsReserved(Guid orderId, IDictionary<Guid, int> products)
        {
            OrderId = orderId;
            Products = products;
        }
    }
}