using System;
using DShop.Common.Messages;
using Newtonsoft.Json;

namespace DShop.Services.Operations.Messages.Events
{
    public class OperationPending : IEvent
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Name { get; }

        [JsonConstructor]
        public OperationPending(Guid id,
            Guid userId, string name)
        {
            Id = id;
            UserId = userId;
            Name = name;
        }
    }
}