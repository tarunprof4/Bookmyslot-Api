using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Authentication.Facebook.Contracts
{
    public class FacebookUserInfoError
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }

    public class Error
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
