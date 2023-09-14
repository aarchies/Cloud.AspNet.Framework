using Glasssix.Contrib.EventBus.Events;

namespace Demo.Application.IntegrationEvents
{
    public record PersonSyncMessagesEvent : IntegrationEvent
    {
        public string? Data { get; set; }
    }
}