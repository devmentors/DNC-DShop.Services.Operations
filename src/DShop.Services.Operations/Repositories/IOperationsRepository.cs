using System;
using System.Threading.Tasks;
using DShop.Services.Operations.Domain;

namespace DShop.Services.Operations.Repositories
{
    public interface IOperationsRepository
    {
        Task<Operation> GetAsync(Guid id);
        Task CreateAsync(Operation operation);
        Task UpdateAsync(Operation operation);
    }
}