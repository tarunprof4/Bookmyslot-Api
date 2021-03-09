using Newtonsoft.Json;

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
