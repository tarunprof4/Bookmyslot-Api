

using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Bookmyslot.Api.Common
{
    public class BaseApiController : ControllerBase
    {
        private IActionResult InternalServerError<T>(Response<T> response)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, response.Messages.First());
        }
        protected virtual IActionResult CreateGetHttpResponse<T>(Response<T> response)
        {
            if (response.ResultType == ResultType.Success)
            {
                if (response.HasResult)
                    return this.Ok(response.Result);

                else
                    return StatusCode(StatusCodes.Status404NotFound, AppBusinessMessages.NoRecordsFound);
            }

            else if (response.ResultType == ResultType.ValidationError)
            {
                return this.BadRequest(response.Messages.First());
            }

            return InternalServerError(response);
        }


        protected virtual IActionResult CreatePostHttpResponse<T>(Response<T> response)
        {
            if (response.ResultType == ResultType.Success)
            {
                return this.Created("", response.Result);
            }

            else if (response.ResultType == ResultType.ValidationError)
            {
                return this.BadRequest(response.Messages.First());
            }

            return InternalServerError(response);
        }

        protected virtual IActionResult CreatePutHttpResponse<T>(Response<T> response)
        {
            if (response.ResultType == ResultType.Success)
            {
                return this.NoContent();
            }

            else if (response.ResultType == ResultType.ValidationError)
            {
                return this.BadRequest(response.Messages.First());
            }

            return InternalServerError(response);
        }


        protected virtual IActionResult CreateDeleteHttpResponse<T>(Response<T> response)
        {
            if (response.ResultType == ResultType.Success)
            {
                return this.NoContent();
            }

            else if (response.ResultType == ResultType.ValidationError)
            {
                return this.BadRequest(response.Messages.First());
            }

            return this.BadRequest(response.Messages.First());
        }
    }
}
