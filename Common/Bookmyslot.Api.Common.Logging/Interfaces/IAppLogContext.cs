using Bookmyslot.Api.Common.Logging.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using System;
using System.Runtime.CompilerServices;

namespace Bookmyslot.Api.Common.Logging.Interfaces
{
    public interface IAppLogContext
    {
        void LogPerf(string identifier, string uri = null, [CallerFilePath] string sourceFilePath = "");

        IDisposable SetSlotModelInfoToContext(SlotModel slotModel);

        IDisposable SetDatabaseRequestLogInfoContext(DatabaseRequestLog databaseRequestLog);
        IDisposable SetDatabaseResponseLogInfoContext(DatabaseResponseLog databaseResponseLog);

        IDisposable SetCustomObjectToContext(dynamic customObject);

    }
}
