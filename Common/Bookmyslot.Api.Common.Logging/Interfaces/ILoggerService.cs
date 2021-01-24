using System;
using System.Runtime.CompilerServices;

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

        void LogFatal(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
             [CallerLineNumber] int sourceLineNumber = 0);

        void LogWarning(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
             [CallerLineNumber] int sourceLineNumber = 0);
    }
}
