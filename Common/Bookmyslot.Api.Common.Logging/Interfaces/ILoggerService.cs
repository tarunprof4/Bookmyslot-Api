    using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Bookmyslot.Api.Common.Logging.Interfaces
{
    public interface ILoggerService
    {
        void LogError(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void LogInfo(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void LogDebug(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
             [CallerLineNumber] int sourceLineNumber = 0);

        void LogVerbose(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
             [CallerLineNumber] int sourceLineNumber = 0);

        void LogCritical(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
             [CallerLineNumber] int sourceLineNumber = 0);

        void LogWarning(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
             [CallerLineNumber] int sourceLineNumber = 0);
    }
}
