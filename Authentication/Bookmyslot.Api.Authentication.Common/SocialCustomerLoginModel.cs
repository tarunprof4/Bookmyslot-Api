using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Authentication.Common
{
    public class SocialCustomerLoginModel
    {
        public string Provider { get; set; }
        public string IdToken { get; set; }

        public string AuthToken { get; set; }
    }
}
