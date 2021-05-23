﻿using System;

namespace Bookmyslot.Api.Common.Logging.Interfaces
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
