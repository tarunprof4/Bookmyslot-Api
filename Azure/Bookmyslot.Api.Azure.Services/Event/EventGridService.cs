using Bookmyslot.Api.Azure.Contracts.Configuration;
using Bookmyslot.Api.Common.Contracts.Event;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.EventGrid;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Logging;
using Bookmyslot.Api.Common.Logging.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Azure.Services.Event
{
    public class EventGridService : IEventGridService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AzureConfiguration azureConfiguration;
        private readonly TopicCredentials topicCredentials;
        private readonly EventGridClient eventGridClient;
        private readonly ILoggerService loggerService;

        public EventGridService(IHttpContextAccessor httpContextAccessor,
            AzureConfiguration azureConfiguration, ILoggerService loggerService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.azureConfiguration = azureConfiguration;
            this.topicCredentials = new TopicCredentials(azureConfiguration.BmsTopicKey);
            this.eventGridClient = new EventGridClient(this.topicCredentials);
            this.loggerService = loggerService;
        }

        public async Task PublishEventAsync(string eventName, BaseDomainEvent baseDomainEvent)
        {
            var coorelationId = httpContextAccessor.HttpContext.Request.Headers[LogConstants.CoorelationId];
            var eventGridLog = new EventGridLog(coorelationId, eventName, baseDomainEvent);
            this.loggerService.Debug("PublishEventsAsync Started {@eventGridLog}", eventGridLog);
            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            await this.eventGridClient.PublishEventsAsync(new Uri(GetTopicName(eventName)).Host, CreateEventGridEvent(baseDomainEvent));
            stopWatch.Stop();

            this.loggerService.Debug("PublishEventsAsync Ended {@eventGridLog}", eventGridLog);
        }


        private string GetTopicName(string eventName)
        {
            string topicEndpoint = azureConfiguration.BmsTopic;
            return topicEndpoint;
        }

        private IList<EventGridEvent> CreateEventGridEvent(BaseDomainEvent baseDomainEvent)
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            eventsList.Add(new EventGridEvent()
            {
                Id = Guid.NewGuid().ToString(),
                EventType = "Contoso.Items.ItemReceived",
                Data = baseDomainEvent,
                EventTime = DateTime.Now,
                Subject = "Door1",
                DataVersion = "2.0"
            });

            return eventsList;
        }
    }
}
