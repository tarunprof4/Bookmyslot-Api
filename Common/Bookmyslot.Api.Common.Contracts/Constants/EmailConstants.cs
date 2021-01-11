using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Common.Contracts.Constants
{
    public class EmailConstants
    {
        public const string EmailFromAddressIsRequired = "Email From Address is Required";
        public const string EmailFromAddressIsInvalid = "Email From Address {0} is InValid";
        public const string EmailToAddressIsRequired = "Email To Address is Required";
        public const string EmailToAddressIsInvalid = "Email To Address {0} is Invalid";
        public const string EmailccAddressIsInvalid = "Email cc Address is Invalid";
        public const string EmailBccAddressIsInvalid= "Email Bcc Address is Invalid";
        public const string EmailSubjectIsRequired = "Email Subject is Required";
        public const string EmailBodyIsRequired = "Email Body is Required";

        public const string SendEmailFailure = "Sending Email Failed";
    }
}
