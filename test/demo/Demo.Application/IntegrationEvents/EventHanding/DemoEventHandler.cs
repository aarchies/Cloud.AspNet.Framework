using Glasssix.Contrib.EventBus.Abstractions;
using Microsoft.Extensions.Logging;

namespace Demo.Application.IntegrationEvents.EventHanding
{
    public class PersonSyncEventHandler : IIntegrationEventHandler<PersonSyncMessagesEvent>
    {
        private readonly ILogger<PersonSyncEventHandler> _logger;

        public PersonSyncEventHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PersonSyncEventHandler>();
        }

        public Task Handle(PersonSyncMessagesEvent @event)
        {
            _logger.LogInformation("Subscribe EventType:{EventType} EventId:{EventId}", "PersonSyncEventHandler", @event.Id);

            return Task.CompletedTask;
        }
    }
}