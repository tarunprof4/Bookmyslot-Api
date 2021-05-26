using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Serilog;
using System;

namespace Bookmyslot.Api.Common.Logging
{
    public class LoggerService : ILoggerService
    {
        public void Information(string messageTemplate)
        {
            Log.Information(messageTemplate);
        }

        public void Information<T>(string messageTemplate, T propertyValue)
        {
            Log.Information(messageTemplate, propertyValue);
        }

        public void Debug(string messageTemplate)
        {
            Log.Debug(messageTemplate);
        }
        public void Debug<T>(string messageTemplate, T propertyValue)
        {
            Log.Debug(messageTemplate, propertyValue);
        }

        public void Error(Exception exception, string messageTemplate)
        {
            Log.Error(exception, messageTemplate);
        }

        public void Error<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            Log.Error(exception, messageTemplate, propertyValue);
        }
    }
}
