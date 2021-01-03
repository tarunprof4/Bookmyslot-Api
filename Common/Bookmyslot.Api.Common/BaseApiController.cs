

using Bookmyslot.Api.Common.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookmyslot.Api.Common
{
    public class BaseApiController : ControllerBase
    {
        private IActionResult HandleUnSuccessfulResponse<T>(Response<T> response)
        {
            if (response.ResultType == ResultType.Empty)
            {
                return StatusCode(StatusCodes.Status404NotFound, response.Messages);
            }

            else if (response.ResultType == ResultType.ValidationError)
            {
                return this.BadRequest(response.Messages);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, response.Messages);
        }
        protected virtual IActionResult CreateGetHttpResponse<T>(Response<T> response)
        {
            if (response.ResultType == ResultType.Success)
            {
                return this.Ok(response.Result);

            }

            return HandleUnSuccessfulResponse(response);
        }


        protected virtual IActionResult CreatePostHttpResponse<T>(Response<T> response)
        {
            
            if (response.ResultType == ResultType.Success)
            {
                return this.Created(string.Empty, response.Result);
            }

            return HandleUnSuccessfulResponse(response);
        }

        protected virtual IActionResult CreatePutHttpResponse<T>(Response<T> response)
        {
            if (response.ResultType == ResultType.Success)
            {
                return this.NoContent();
            }

            return HandleUnSuccessfulResponse(response);
        }


        protected virtual IActionResult CreateDeleteHttpResponse<T>(Response<T> response)
        {
            if (response.ResultType == ResultType.Success)
            {
                return this.NoContent();
            }

            return HandleUnSuccessfulResponse(response);
        }
    }
}
