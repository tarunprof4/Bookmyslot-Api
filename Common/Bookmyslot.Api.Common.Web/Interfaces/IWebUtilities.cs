using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Common.Web.Interfaces
{
    public interface IWebUtilities
    {
        HttpContext GetHttpContext();
    }
}
