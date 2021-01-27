using Bookmyslot.Api.Common.Logging.Contracts;
using Bookmyslot.Api.Common.Logging.Interfaces;
using Bookmyslot.Api.Common.Logging.LogContexts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Serilog.Context;
using System;
using System.Runtime.CompilerServices;

namespace Bookmyslot.Api.Common.Logging
{
    public class AppLogContext : IAppLogContext
    {
        public void LogPerf(string identifier, string uri = null, [CallerFilePath] string sourceFilePath = "")
        {
            throw new NotImplementedException();
        }

        public IDisposable SetCustomObjectToContext(dynamic customObject)
        {
            throw new Exception();
            //CustomLogContext customLogContext = new CustomLogContext(customObject);
            //return LogContext.Push(customLogContext);
        }

        public IDisposable SetDatabaseRequestLogInfoContext(DatabaseRequestLog databaseRequestLog)
        {
            var databaseRequestLogContext = new DatabaseRequestLogContext(databaseRequestLog);
            return LogContext.Push(databaseRequestLogContext);
        }

        public IDisposable SetDatabaseResponseLogInfoContext(DatabaseResponseLog databaseResponseLog)
        {
            var databaseResponseLogContext = new DatabaseResponseLogContext(databaseResponseLog);
            return LogContext.Push(databaseResponseLogContext);
        }

        public IDisposable SetSlotModelInfoToContext(SlotModel slotModel)
        {
            SlotLogContext slotLogContext = new SlotLogContext(slotModel);
            return LogContext.Push(slotLogContext);
        }
    }
}
