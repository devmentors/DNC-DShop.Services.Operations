using System;
using System.Threading.Tasks;
using DShop.Services.Operations.Dto;

namespace DShop.Services.Operations.Services
{
    public interface IOperationsStorage
    {
        Task<OperationDto> GetAsync(Guid id);

        Task SetAsync(Guid id, Guid userId, string name,  OperationState state, 
            string resource, string code = null, string reason = null);
    }
}