using System;
using System.Collections.Generic;
using DShop.Common.Messages;
using Newtonsoft.Json;

namespace DShop.Services.Operations.Messages.Products.Commands
{
    [MessageNamespace("products")]
    public class ReserveProducts : ICommand
    {
        public Guid OrderId { get; set; }
        public IDictionary<Guid, int> Products { get; }
        
        [JsonConstructor]
        public ReserveProducts(Guid orderId, IDictionary<Guid, int> products)
        {
            OrderId = orderId;
            Products = products;
        }
    }
}