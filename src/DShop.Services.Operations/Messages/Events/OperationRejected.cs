using System;
using DShop.Common.Messages;
using Newtonsoft.Json;

namespace DShop.Services.Operations.Messages.Events
{
    public class OperationRejected : IEvent
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string Code { get; }
        public string Message { get; }

        [JsonConstructor]
        public OperationRejected(Guid id,
            Guid userId, string name,
            string code, string message)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Code = code;
            Message = message;
        }
    }
}