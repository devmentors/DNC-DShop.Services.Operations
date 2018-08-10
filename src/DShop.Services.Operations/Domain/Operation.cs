using System;
using DShop.Common.Types;

namespace DShop.Services.Operations.Domain
{
    public class Operation : IIdentifiable
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public Guid UserId { get; protected set; }
        public string Origin { get; protected set; }
        public string Resource { get; protected set; }
        public string State { get; protected set; }
        public string Message { get; protected set; }
        public string Code { get; protected set; }
        public bool Completed => State == States.Completed;
        public bool Success => Completed && Code == Codes.Success;
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }

        protected Operation()
        {
        }

        public Operation(Guid id, string name, Guid userId,
            string origin, string resource, DateTime createdAt)
        {
            Id = id;
            Name = name;
            UserId = userId;
            Origin = origin;
            Resource = resource;
            CreatedAt = createdAt;
            State = States.Created;
        }

        public void Complete(string message = null)
        {
            if (State == States.Rejected)
            {
                throw new InvalidOperationException($"Operation: {Id} has been rejected and can not be completed.");
            }
            SetCode(Codes.Success);
            SetMessage(message);
            SetState(States.Completed);
        }

        public void Reject(string code, string message)
        {
            if (State == States.Completed)
            {
                throw new InvalidOperationException($"Operation: {Id} has been completed and can not be rejected.");
            }
            SetCode(code);
            SetMessage(message);
            SetState(States.Rejected);
        }

        public void SetMessage(string message)
        {
            Message = message;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetState(string state)
        {
            if (State == state)
            {
                return;
            }
            State = state;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetCode(string code)
        {
            if (Code == code)
            {
                return;
            }
            Code = code;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}