using Autofac;
using DShop.Common.Messages;
using System.Linq;
using System.Reflection;
using Chronicle;

namespace DShop.Services.Operations.Sagas
{
    internal static class Extensions
    {
        internal static bool IsProcessable<TMessage>(this TMessage message) where TMessage : IMessage
            => Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Any(t => t.IsAssignableTo<ISagaAction<TMessage>>());
    }
}
