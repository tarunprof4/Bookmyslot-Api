using Bookmyslot.Api.Authentication.Common;

namespace Bookmyslot.Api.Customers.Domain
{
    public class CustomerSummaryModel
    {
        public string Id { get; }

        public string FullName { get; }

        public string Email { get; }

        public CustomerSummaryModel(CurrentUserModel currentUserModel)
        {
            Id = currentUserModel.Id;
            FullName = currentUserModel.GetFullName();
            Email = currentUserModel.Email;
        }
     
    }
}
