namespace Bookmyslot.Api.Customers.Contracts
{
    public class CustomerModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BioHeadLine { get; set; }

        public bool IsVerified { get; set; }
    }
}
