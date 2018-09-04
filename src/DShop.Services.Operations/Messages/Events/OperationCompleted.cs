using System;
using DShop.Common.Messages;
using Newtonsoft.Json;

namespace DShop.Services.Operations.Messages.Events
{
    public class OperationCompleted : IEvent
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Name { get; }

        [JsonConstructor]
        public OperationCompleted(Guid id,
            Guid userId, string name)
        {
            Id = id;
            UserId = userId;
            Name = name;
        }
    }
}