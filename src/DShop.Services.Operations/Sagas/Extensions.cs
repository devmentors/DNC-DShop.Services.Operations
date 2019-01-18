using System;
using Autofac;
using DShop.Common.Messages;
using System.Linq;
using System.Reflection;
using Chronicle;

namespace DShop.Services.Operations.Sagas
{
    internal static class Extensions
    {
        private static readonly Type[] SagaTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsAssignableTo<ISaga>())
            .ToArray();

        internal static bool BelongsToSaga<TMessage>(this TMessage _) where TMessage : IMessage
            => SagaTypes.Any(t => t.IsAssignableTo<ISagaAction<TMessage>>());
    }
}
