using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.ValueObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookmyslot.Api.Web.Common
{
    public class BaseApiController : ControllerBase
    {
        private IActionResult HandleUnSuccessfulResponse<T>(Result<T> response)
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
        protected virtual IActionResult CreateGetHttpResponse<T>(Result<T> response)
        {
            if (response.ResultType == ResultType.Success)
            {
                return this.Ok(response.Value);

            }

            return HandleUnSuccessfulResponse(response);
        }


        protected virtual IActionResult CreatePostHttpResponse<T>(Result<T> response)
        {

            if (response.ResultType == ResultType.Success)
            {
                return this.Created(string.Empty, response.Value);
            }

            return HandleUnSuccessfulResponse(response);
        }

        protected virtual IActionResult CreatePutHttpResponse<T>(Result<T> response)
        {
            if (response.ResultType == ResultType.Success)
            {
                return this.NoContent();
            }

            return HandleUnSuccessfulResponse(response);
        }


        protected virtual IActionResult CreateDeleteHttpResponse<T>(Result<T> response)
        {
            if (response.ResultType == ResultType.Success)
            {
                return this.NoContent();
            }

            return HandleUnSuccessfulResponse(response);
        }
    }
}
