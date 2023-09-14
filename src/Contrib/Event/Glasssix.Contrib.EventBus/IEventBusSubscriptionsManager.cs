using Glasssix.Contrib.EventBus.Abstractions;
using Glasssix.Contrib.EventBus.Events;
using System;
using System.Collections.Generic;
using static EventBus.InMemoryEventBusSubscriptionsManager;

namespace Glasssix.Contrib.EventBus
{
    public interface IEventBusSubscriptionsManager
    {
        event EventHandler<string> OnEventRemoved;

        bool IsEmpty { get; }

        void AddDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void AddTopicSubscription<T, TH>(string queueName)
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

        void Clear();

        string GetEventKey<T>();

        Type GetEventTypeByName(string eventName);

        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent;

        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;

        bool HasSubscriptionsForEvent(string eventName);

        void RemoveDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        void RemoveSubscription<T, TH>()
                                                where TH : IIntegrationEventHandler<T>
                where T : IntegrationEvent;
    }
}