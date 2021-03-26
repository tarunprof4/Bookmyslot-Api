using Newtonsoft.Json;

namespace Bookmyslot.Api.Authentication.Common
{
    public class CurrentUserModel
    {
        public string Id { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BioHeadLine { get; set; }

        public bool IsVerified { get; set; }

        public string ProfilePictureUrl { get; set; }

        
        public string UserName { get; set; }

        [JsonIgnore]
        public string Email { get; set; }

    }
}
