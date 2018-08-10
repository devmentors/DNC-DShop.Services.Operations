using System;
using System.Threading.Tasks;
using DShop.Services.Operations.Dto;

namespace DShop.Services.Operations.Services
{
    public interface IOperationsService
    {
        Task<OperationDto> GetAsync(Guid id);
        Task CreateAsync(Guid id, string name, Guid userId, 
            string origin, string resource, DateTime createdAt);
        Task RejectAsync(Guid id, string code, string message);
        Task CompleteAsync(Guid id, string message = null);
    }
}