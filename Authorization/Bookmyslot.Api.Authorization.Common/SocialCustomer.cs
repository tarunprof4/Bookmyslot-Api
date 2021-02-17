namespace Bookmyslot.Api.Authorization.Common
{
    public class SocialCustomer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Provider { get; set; }
        public string Token { get; set; }
    }
}
