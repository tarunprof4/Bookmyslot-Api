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
            var eventRequestLog = new EventRequestLog(coorelationId, eventName, baseDomainEvent);
            this.loggerService.Debug("PublishEventsAsync Started {@eventGridLog}", eventRequestLog);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                await this.eventGridClient.PublishEventsAsync(new Uri(GetTopicName(eventName)).Host, CreateEventGridEvent(baseDomainEvent));
            }

            catch (Exception exp)
            {
                this.loggerService.Error(exp, "{@eventGridLog}", eventRequestLog);
            }

            finally
            {
                stopWatch.Stop();
                var eventCompleteLog = new EventCompleteLog(coorelationId, stopWatch.Elapsed);
                this.loggerService.Debug("PublishEventsAsync Completed {@eventCompleteLog}", eventCompleteLog);
            }


            
        }


        private string GetTopicName(string eventName)
        {
            try
            {
                var mailMessage = CreateMailMessage(emailModel);
                await this.smtpClient.SendMailAsync(mailMessage);
                return Response<bool>.Success(true);
            }
            catch (Exception exp)
            {
                var coorelationId = httpContextAccessor.HttpContext.Request.Headers[LogConstants.CoorelationId];
                var emaillog = new EmailLog(coorelationId);
                this.loggerService.Error(exp, "{@emaillog}", emaillog);
                return Response<bool>.Error(new List<string>() { EmailConstants.SendEmailFailure });
            }


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
