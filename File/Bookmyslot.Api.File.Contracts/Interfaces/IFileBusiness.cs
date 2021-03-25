using Bookmyslot.Api.Common.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.File.Contracts.Interfaces
{
    public interface IFileBusiness
    {
        Response<bool> IsImageValid(IFormFile formFile);
    }
}
