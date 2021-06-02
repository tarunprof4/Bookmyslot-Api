using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.EventGrid;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Customers.Domain.Constants;
using Bookmyslot.Api.Customers.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business.EventHandlers
{

    public class CustomerRegisteredNotificationHandler : INotificationHandler<CustomerRegisteredDomainEvent>
    {
        private readonly IEventGridService eventGridService;
        private readonly ILoggerService loggerService;
        public CustomerRegisteredNotificationHandler(IEventGridService eventGridService, ILoggerService loggerService)
        {
            this.eventGridService = eventGridService;
            this.loggerService = loggerService;
        }

        public async Task Handle(CustomerRegisteredDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            this.loggerService.Debug("CustomerRegisteredNotificationHandler started");
            await this.eventGridService.PublishEventAsync(EventConstants.CustomerRegisteredEvent, domainEvent);
            this.loggerService.Debug("CustomerRegisteredNotificationHandler ended");
        }
    }
}
