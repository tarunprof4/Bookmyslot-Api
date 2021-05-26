using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Compression;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Logging;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Logging;
using Bookmyslot.Api.Common.Logging.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.IO;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;


namespace Bookmyslot.Api.Web.Common
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly ICompression _compression;
        private readonly ILoggerService loggerService;


        public RequestResponseLoggingMiddleware(RequestDelegate next, ICompression compression, ILoggerService loggerService)
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            _compression = compression;
            this.loggerService = loggerService;
        }

        public async Task Invoke(HttpContext context)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var correlationId = context.Request.Headers[ThirdPartyConstants.CoorelationId];
            var requestId = Guid.NewGuid().ToString();
            context.Request.Headers.Add(LogConstants.RequestId, requestId);

            var requestBody = await LogRequest(context);
            var requestLog = CreateRequestLog(context, correlationId, requestId, requestBody);
            this.loggerService.Debug("Http Request {@httpRequest}", requestLog);

            var compresedBody = await LogResponse(context);
            stopWatch.Stop();
            var responseLog = CreateResponseLog(context, correlationId, requestId, compresedBody, stopWatch.Elapsed);
            this.loggerService.Debug("Http Response {@httpResponse}", responseLog);
        }


        private async Task<string> LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            var requestBody = ReadStreamInChunks(requestStream);

            context.Request.Body.Position = 0;
            return requestBody;
        }

        private async Task<string> LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;
            await _next(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var compresedBody = string.Empty;
            if (!string.IsNullOrWhiteSpace(responseBodyText))
            {
                compresedBody = _compression.Compress(responseBodyText);
            }
            context.Response.Body.Seek(0, SeekOrigin.Begin);

         
           
            await responseBody.CopyToAsync(originalBodyStream);
            return compresedBody;
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                                                   0,
                                                   readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);
            return textWriter.ToString();
        }

        private RequestLog CreateRequestLog(HttpContext context, string correlationId, string requestId, string requestBody)
        {
            var requestLog = new RequestLog();
            requestLog.RequestId = requestId;
            requestLog.CorrelationId = correlationId;
            requestLog.Schema = context.Request.Scheme;
            requestLog.Host = context.Request.Host;
            requestLog.Path = context.Request.Path;
            requestLog.Method = context.Request.Method;
            requestLog.QueryString = context.Request.QueryString;
            requestLog.Body = requestBody;
            requestLog.IpAddress = context.Connection.RemoteIpAddress.ToString();
            requestLog.UserAgent = context.Request.Headers[LogConstants.UserAgent].ToString();

            return requestLog;
        }

        private ResponseLog CreateResponseLog(HttpContext context, string correlationId, string requestId, string responseCompressedBody, TimeSpan responseTime)
        {
            var responseLog = new ResponseLog();
            responseLog.RequestId = requestId;
            responseLog.CorrelationId = correlationId;

            responseLog.StatusCode = context.Response.StatusCode;
            responseLog.CompressedBody = responseCompressedBody;
            responseLog.ResponseTime = responseTime;
            return responseLog;
        }
    }

}