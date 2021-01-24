using Bookmyslot.Api.Common.Contracts.Interfaces;
using Bookmyslot.Api.Common.Logging.Enrichers;
using Bookmyslot.Api.Common.Logging.Interfaces;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Bookmyslot.Api.Common.Logging
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger serilogFileLogger;
        private readonly IAppConfiguration appConfiguration;
        public LoggerService(IAppConfiguration appConfiguration)
        {
            this.appConfiguration = appConfiguration;
            this.serilogFileLogger = new LoggerConfiguration().Enrich.FromLogContext()
         .MinimumLevel.Verbose()
         .Enrich.With(new DefaultLogEnricher(this.appConfiguration))
         //.WriteTo.Http("http://localhost:9600/")
         //.WriteTo.Async(a => a.Http("http://localhost:9600/", 1))

          .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day,
         outputTemplate: "{Message} {Properties} {NewLine}  {Exception}")
         .CreateLogger();
        }

        public void LogFatal(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            using (AddCallerDetailsToLogContext(sourceFilePath, memberName, sourceLineNumber))
            {
                this.serilogFileLogger.Fatal(ex, message);
            }
        }

        public void LogDebug(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            using (AddCallerDetailsToLogContext(sourceFilePath, memberName, sourceLineNumber))
            {
                this.serilogFileLogger.Debug(ex, message);
            }
        }

        public void LogError(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            using (AddCallerDetailsToLogContext(sourceFilePath, memberName, sourceLineNumber))
            {
                this.serilogFileLogger.Error(ex, message);
            }
        }

        public void LogInfo(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            using (AddCallerDetailsToLogContext(sourceFilePath, memberName, sourceLineNumber))
            {
                this.serilogFileLogger.Information(ex, message);
            }
        }

        public void LogVerbose(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            using (AddCallerDetailsToLogContext(sourceFilePath, memberName, sourceLineNumber))
            {
                this.serilogFileLogger.Verbose(ex, message);
            }
        }

        public void LogWarning(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            using (AddCallerDetailsToLogContext(sourceFilePath, memberName, sourceLineNumber))
            {
                this.serilogFileLogger.Warning(ex, message);
            }
        }


        private IDisposable AddCallerDetailsToLogContext(string sourceFilePath, string memberName, int sourceLineNumber)
        {
            string fileName;
            try
            {
                fileName = Path.GetFileName(sourceFilePath);
            }
            catch (Exception)
            {
                fileName = string.Empty;
            }
            var callerInfo = new
            {
                FilePath = fileName,
                Method = memberName,
                LineNumber = sourceLineNumber
            };
            return LogContext.PushProperty("CallerDetails", callerInfo, true);
        }
    }
}
