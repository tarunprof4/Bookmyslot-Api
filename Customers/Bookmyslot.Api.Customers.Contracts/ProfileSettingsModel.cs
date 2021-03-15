using Bookmyslot.Api.Common.Contracts.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookmyslot.Api.Customers.Contracts
{
    public class ProfileSettingsModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Gender { get; set; }
    }
}
