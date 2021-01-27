using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Logging;
using Bookmyslot.Api.Common.Logging.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.IO;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;


namespace Bookmyslot.Api.Common
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly ICompression _compression;


        public RequestResponseLoggingMiddleware(RequestDelegate next, ICompression compression)
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            _compression = compression;
        }

        public async Task Invoke(HttpContext context)
        {
            var correlationId = context.Request.Headers[ThirdPartyConstants.CoorelationId];
            var requestId = Guid.NewGuid().ToString();
            context.Request.Headers.Add(LogConstants.RequestId, requestId);

            await LogRequest(context, correlationId,  requestId);
            await LogResponse(context, correlationId, requestId);
        }


        private async Task LogRequest(HttpContext context, string correlationId, string requestId)
        {
            context.Request.EnableBuffering();

            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            var requestBody = ReadStreamInChunks(requestStream);

            var requestLog = CreateRequestLog(context, correlationId, requestId, requestBody);
            Log.Debug("Http Request {@httpRequest}", requestLog);
        
            context.Request.Body.Position = 0;
        }

        private async Task LogResponse(HttpContext context, string correlationId, string requestId)
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

            var responseLog = CreateResponseLog(context, correlationId, requestId, compresedBody);
            Log.Debug("Http Response {@httpResponse}", responseLog);
           
            await responseBody.CopyToAsync(originalBodyStream);
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

        private ResponseLog CreateResponseLog(HttpContext context, string correlationId, string requestId, string responseCompressedBody)
        {
            var responseLog = new ResponseLog();
            responseLog.RequestId = requestId;
            responseLog.CorrelationId = correlationId;

            responseLog.StatusCode = context.Response.StatusCode;
            responseLog.CompressedBody = responseCompressedBody;
            return responseLog;
        }
    }

}