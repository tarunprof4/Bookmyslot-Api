using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts.Constants;
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
            var requestId = Guid.NewGuid().ToString();
            context.Request.Headers.Add(LogConstants.RequestId, requestId);

            await LogRequest(context, requestId);
            await LogResponse(context, requestId);
        }


        private async Task LogRequest(HttpContext context, string requestId)
        {
            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            var requestBody = ReadStreamInChunks(requestStream);
            var compresedBody = string.Empty;
            if (!string.IsNullOrWhiteSpace(requestBody))
            {
                compresedBody = _compression.Compress(requestBody);
            }

            Log.Information($"Http Request Information:{Environment.NewLine}" +
                                   $"Request Id: {requestId} " +
                                   $"Executing At: {DateTime.UtcNow} " +
                                   $"Schema:{context.Request.Scheme} " +
                                   $"Host: {context.Request.Host} " +
                                   $"Path: {context.Request.Path} " +
                                   $"Method Type: {context.Request.Method} " +
                                   $"QueryString: {context.Request.QueryString} " +
                                   $"Request Body: {requestBody} " +
                                   $"Compressed Body: {compresedBody} "
                                   );
            context.Request.Body.Position = 0;
        }

        private async Task LogResponse(HttpContext context, string requestId)
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
            Log.Information($"Http Response Information:{Environment.NewLine}" +
                                   $"Request Id: {requestId} " +
                                   $"Executed At: {DateTime.UtcNow} " +
                                   $"Schema:{context.Request.Scheme} " +
                                   $"Host: {context.Request.Host} " +
                                   $"Path: {context.Request.Path} " +
                                   $"Method Type: {context.Request.Method} " +
                                   $"QueryString: {context.Request.QueryString} " +
                                   $"Response Status Code: {context.Response.StatusCode} " +
                                   $"Response Body: {responseBodyText} " +
                                   $"Compressed Body: {compresedBody} "
                                   );
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
    }
}