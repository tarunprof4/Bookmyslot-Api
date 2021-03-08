using Bookmyslot.Api.Common.Contracts;
using Marvin.StreamExtensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Common
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<Response<T>> HandleError<T>(this HttpResponseMessage httpResponseMessage)
        {
            var errorStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var errors = errorStream.ReadAndDeserializeFromJson<List<string>>();

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return Response<T>.Empty(errors);
            }

            else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return Response<T>.ValidationError(errors);
            }

            return Response<T>.Error(errors);
        }
    }
}
