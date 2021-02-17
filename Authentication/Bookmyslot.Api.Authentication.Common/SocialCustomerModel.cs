namespace Bookmyslot.Api.Authentication.Common
{
    public class SocialCustomerModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Provider { get; set; }
        public string Token { get; set; }
    }
}
