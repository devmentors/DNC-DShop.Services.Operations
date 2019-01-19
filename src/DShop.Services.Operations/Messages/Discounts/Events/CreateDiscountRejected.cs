using DShop.Common.Messages;
using Newtonsoft.Json;

namespace DShop.Services.Operations.Messages.Discounts.Events
{
    [MessageNamespace("discounts")]
    public class CreateDiscountRejected : IRejectedEvent
    {
        public string Reason { get; }
        public string Code { get; }

        [JsonConstructor]
        public CreateDiscountRejected(string reason, string code)
        {
            Reason = reason;
            Code = code;
        }
    }
}