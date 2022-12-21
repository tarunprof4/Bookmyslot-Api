using System;

namespace Bookmyslot.SharedKernel.Contracts.Logging
{
    public interface ILoggerService
    {
        void Information(string messageTemplate);
        void Information<T>(string messageTemplate, T propertyValue);
        void Debug(string messageTemplate);
        void Debug<T>(string messageTemplate, T propertyValue);

        void Error(Exception exception, string messageTemplate);
        void Error<T>(Exception exception, string messageTemplate, T propertyValue);
    }
}
