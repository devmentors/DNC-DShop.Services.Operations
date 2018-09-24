using Chronicle;
using DShop.Services.Operations.Messages.Customers.Events;
using DShop.Services.Operations.Messages.Identity.Events;
using DShop.Services.Operations.Messages.Orders.Commands;
using DShop.Services.Operations.Services;
using System;
using System.Threading.Tasks;

namespace DShop.Services.Operations.Sagas
{
    public class NewCustomerDiscountSaga : Saga<NewCustomerDiscountSagaState>,
        ISagaStartAction<SignedUp>,
        ISagaAction<CustomerCreated>,
        ISagaAction<CreateOrder>

    {
        private const int CreationHoursLimit = 24;
        private readonly IOperationsStorage _operationStorage;
        private readonly IOperationPublisher _operationPublisher;

        public NewCustomerDiscountSaga(
            IOperationsStorage operationStorage,
            IOperationPublisher operationPublisher)
        {
            _operationStorage = operationStorage;
            _operationPublisher = operationPublisher;
        }

        public Task HandleAsync(SignedUp message)
        {
            Data.UserCreatedDate = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public Task HandleAsync(CustomerCreated message)
        {
            var diff = DateTime.UtcNow.Subtract(Data.UserCreatedDate);

            if (diff.TotalHours <= CreationHoursLimit)
            {
                Data.CustomerCreatedDate = DateTime.UtcNow;
            }
            else
            {
                Reject();
            }

            return Task.CompletedTask;
        }

        public Task HandleAsync(CreateOrder message)
        {
            var diff = DateTime.UtcNow.Subtract(Data.CustomerCreatedDate);

            if (diff.TotalHours <= CreationHoursLimit)
            {
                Complete();
            }
            else
            {
                Reject();
            }

            return Task.CompletedTask;
        }

        public async Task CompensateAsync(SignedUp message) { }

        public async Task CompensateAsync(CustomerCreated message) { }

        public async Task CompensateAsync(CreateOrder message) { }
    }

    public class NewCustomerDiscountSagaState
    {
        public DateTime UserCreatedDate { get; set; }
        public DateTime CustomerCreatedDate { get; set; }
    }
}
