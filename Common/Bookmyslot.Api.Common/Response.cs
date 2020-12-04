using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Common
{
    public class Response<T> where T : class
    {
        public T Result { get;  set; }
        public ResultType ResultType { get; set; }
        public string Message { get; set; }

        public Response()
        {
            this.ResultType = ResultType.Success;
        }
        public bool HasResult
        {
            get
            {
                return this.Result != null;
            }
        }

        public static Response<T> Success(T result)
        {
            var response = new Response<T> { ResultType = ResultType.Success, Result = result };

            return response;
        }


        public static  Response<T> Failed(string errorMessage)
        {
            var response = new Response<T> { ResultType = ResultType.Error, Message = errorMessage };

            return response;
        }

        public static Response<T> ValidationError(string validationMessage)
        {
            var response = new Response<T>
            {
                ResultType = ResultType.ValidationError,
                Message = validationMessage,
            };

            return response;
        }

    }
}
