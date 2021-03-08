using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Authentication.Facebook.Contracts
{
    public class FacebookTokenValidation
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }
    }
}
