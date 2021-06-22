using Bookmyslot.BackgroundTasks.Api.Contracts;

namespace Bookmyslot.BackgroundTasks.Api.Emails.ViewModels
{
    public class RegisterCustomerViewModel
    {
        public string FirstName { get; }
        public string LastName { get; }

        public string Email { get; }

        public RegisterCustomerViewModel(CustomerModel customerModel)
        {
            this.FirstName = customerModel.FirstName;
            this.LastName = customerModel.LastName;
            this.Email = customerModel.Email;
        }


    }
}
