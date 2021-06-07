using Bookmyslot.Api.Common.Contracts.Event;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.EventGrid
{
    public interface IEventGridService
    {
        Task PublishEventAsync(IntegrationEvent integrationEvent);
    }
}
