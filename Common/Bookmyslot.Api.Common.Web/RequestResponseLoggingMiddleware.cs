using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.SharedKernel.Contracts.Compression;
using Bookmyslot.SharedKernel.Contracts.Logging;
using Bookmyslot.SharedKernel.Logging.Constants;
using Bookmyslot.SharedKernel.Logging.Contracts;
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

            var uiRequestId = context.Request.Headers[ThirdPartyConstants.UiRequestId];
            var coorelationId = Guid.NewGuid().ToString();
            context.Request.Headers.Add(LogConstants.CoorelationId, coorelationId);

            var requestBody = await LogRequest(context);
            var requestLog = CreateRequestLog(context, uiRequestId, coorelationId, requestBody);
            this.loggerService.Debug("Http Request {@httpRequest}", requestLog);

            var compresedBody = await LogResponse(context);
            stopWatch.Stop();
            var responseLog = CreateResponseLog(context, uiRequestId, coorelationId, compresedBody, stopWatch.Elapsed);
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

        private RequestLog CreateRequestLog(HttpContext context, string requestId, string correlationId, string requestBody)
        {
            var requestLog = new RequestLog
            {
                RequestId = requestId,
                CorrelationId = correlationId,
                Schema = context.Request.Scheme,
                Host = context.Request.Host,
                Path = context.Request.Path,
                Method = context.Request.Method,
                QueryString = context.Request.QueryString,
                Body = requestBody,
                IpAddress = context.Connection.RemoteIpAddress.ToString(),
                UserAgent = context.Request.Headers[LogConstants.UserAgent].ToString()
            };

            return requestLog;
        }

        private ResponseLog CreateResponseLog(HttpContext context, string requestId, string correlationId, string responseCompressedBody, TimeSpan responseTime)
        {
            var responseLog = new ResponseLog
            {
                RequestId = requestId,
                CorrelationId = correlationId,

                StatusCode = context.Response.StatusCode,
                CompressedBody = responseCompressedBody,
                ResponseTime = responseTime
            };
            return responseLog;
        }
    }

}