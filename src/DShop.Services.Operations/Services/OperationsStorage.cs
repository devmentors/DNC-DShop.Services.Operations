using System;
using System.Threading.Tasks;
using DShop.Services.Operations.Dto;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace DShop.Services.Operations.Services
{
    public class OperationsStorage : IOperationsStorage
    {
        private readonly IDistributedCache _cache;
        private static readonly string CompletedState = OperationState.Completed.ToString().ToLowerInvariant();
        private static readonly string RejectedState = OperationState.Rejected.ToString().ToLowerInvariant();

        public OperationsStorage(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<bool> TrySetAsync(Guid id, Guid userId, string name, OperationState state, 
            string resource, string code = null, string reason = null)
        {
            var newState = state.ToString().ToLowerInvariant();
            var operation = await GetAsync(id);
            if (operation != null && CannotUpdate(operation.State, newState))
            {
                return false;
            }

            operation = operation ?? new OperationDto();
            operation.Id = id;
            operation.UserId = userId;
            operation.Name = name;
            operation.State = newState;
            operation.Resource = resource;
            operation.Code = code ?? string.Empty;
            operation.Reason = reason ?? string.Empty;
            
            await _cache.SetStringAsync(id.ToString("N"),
                JsonConvert.SerializeObject(operation),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(1)
                });

            return true;
        }

        private static bool CannotUpdate(string currentSate, string newState)
            => currentSate.Equals(newState, StringComparison.InvariantCultureIgnoreCase) ||
               currentSate.Equals(CompletedState, StringComparison.InvariantCultureIgnoreCase) ||
               currentSate.Equals(RejectedState, StringComparison.InvariantCultureIgnoreCase);

        public async Task<OperationDto> GetAsync(Guid id)
        {
            var operation = await _cache.GetStringAsync(id.ToString("N"));

            return string.IsNullOrWhiteSpace(operation) ? 
                null : 
                JsonConvert.DeserializeObject<OperationDto>(operation);
        }
    }
}