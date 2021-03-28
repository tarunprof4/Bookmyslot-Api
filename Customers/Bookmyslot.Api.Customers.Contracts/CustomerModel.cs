namespace Bookmyslot.Api.Customers.Contracts
{
    public class CustomerModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BioHeadLine { get; set; }

        public bool IsVerified { get; set; }

        public string ProfilePictureUrl { get; set; }


        public string UserName { get; set; }

        public string Email { get; set; }

    }
}
