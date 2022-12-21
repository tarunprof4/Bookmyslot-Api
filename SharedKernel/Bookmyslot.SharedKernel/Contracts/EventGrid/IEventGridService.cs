using Bookmyslot.SharedKernel.Event;
using System.Threading.Tasks;

namespace Bookmyslot.SharedKernel.Contracts.EventGrid
{
    public interface IEventGridService
    {
        Task PublishEventAsync(IntegrationEvent integrationEvent);
    }
}
