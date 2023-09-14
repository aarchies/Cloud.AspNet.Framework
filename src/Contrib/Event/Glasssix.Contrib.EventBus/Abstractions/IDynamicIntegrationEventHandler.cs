using System.Threading.Tasks;

namespace Glasssix.Contrib.EventBus.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}