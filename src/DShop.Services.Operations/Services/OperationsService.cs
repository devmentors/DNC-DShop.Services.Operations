using System;
using System.Threading.Tasks;
using DShop.Services.Operations.Domain;
using DShop.Services.Operations.Dtos;
using DShop.Services.Operations.Repositories;

namespace DShop.Services.Operations.Services
{
    public class OperationsService : IOperationsService
    {
        private readonly IOperationsRepository _operationRepository;

        public OperationsService(IOperationsRepository operationRepository)
        {
            _operationRepository = operationRepository;
        }

        public async Task<OperationDto> GetAsync(Guid id)
        {
            var operation = await _operationRepository.GetAsync(id);

            return operation == null ? null : new OperationDto
            {
                Id = operation.Id,
                Name = operation.Name,
                Resource = operation.Resource,
                Message = operation.Message,
                State = operation.State,
                Completed = operation.State == States.Completed,
                Success = operation.Success
            };
        }

        public async Task CreateAsync(Guid id, string name, Guid userId, 
            string origin, string resource, DateTime createdAt)
        {
            var operation = new Operation(id, name, userId, origin, resource, createdAt);
            await _operationRepository.CreateAsync(operation);
        }

        public async Task RejectAsync(Guid id, string code, string message)
            => await UpdateAsync(id, x => x.Reject(code, message));

        public async Task CompleteAsync(Guid id, string message = null)
            => await UpdateAsync(id, x => x.Complete(message));

        private async Task UpdateAsync(Guid id, Action<Operation> update)
        {
            var operation = await _operationRepository.GetAsync(id);
            update(operation);
            await _operationRepository.UpdateAsync(operation);
        }
    }
}