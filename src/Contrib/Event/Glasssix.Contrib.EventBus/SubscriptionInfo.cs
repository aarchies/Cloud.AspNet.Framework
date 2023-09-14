using Glasssix.Contrib.EventBus;
using System;

namespace EventBus
{
    public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        public class SubscriptionInfo
        {
            private SubscriptionInfo(bool isDynamic, Type handlerType)
            {
                IsDynamic = isDynamic;
                HandlerType = handlerType;
            }

            public Type HandlerType { get; }
            public bool IsDynamic { get; }

            public static SubscriptionInfo Dynamic(Type handlerType) =>
                new SubscriptionInfo(true, handlerType);

            public static SubscriptionInfo Typed(Type handlerType) =>
                new SubscriptionInfo(false, handlerType);
        }
    }
}