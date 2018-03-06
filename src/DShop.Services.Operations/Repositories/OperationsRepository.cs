using System;
using System.Threading.Tasks;
using DShop.Common.Mongo;
using DShop.Services.Operations.Domain;

namespace DShop.Services.Operations.Repositories
{
    public class OperationsRepository : IOperationsRepository
    {
        private readonly IMongoRepository<Operation> _repository;

        public OperationsRepository(IMongoRepository<Operation> repository)
        {
            _repository = repository;
        }

        public async Task<Operation> GetAsync(Guid id)
            => await _repository.GetAsync(id);

        public async Task CreateAsync(Operation operation)
            => await _repository.CreateAsync(operation);

        public async Task UpdateAsync(Operation operation)
            => await _repository.UpdateAsync(operation);
    }
}