﻿using Bookmyslot.Api.Common.Logging.Enrichers;
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
        public LoggerService()
        {
            this.serilogFileLogger = new LoggerConfiguration().Enrich.FromLogContext()
         .MinimumLevel.Verbose()
         .Enrich.WithProperty("Version", "1.0.0")
         .Enrich.With(new ThreadLogEnricher())
         .Enrich.With(new UtcTimestampEnricher())
         //.WriteTo.Http("http://localhost:9600/")
         //.WriteTo.Async(a => a.Http("http://localhost:9600/", 1))

          .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day,
         outputTemplate: "{Timestamp:HH:mm:ss} ({Version}) [{Level}] ({ThreadId}) {Message} {NewLine} {Properties} {Exception}")
         .CreateLogger();
        }

        public void LogCritical(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void LogInfo(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            throw new NotImplementedException();
        }

        public void LogVerbose(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            throw new NotImplementedException();
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
