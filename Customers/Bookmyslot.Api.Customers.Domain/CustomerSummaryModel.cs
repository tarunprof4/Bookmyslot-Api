using Bookmyslot.Api.Authentication.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Customers.Domain
{
    public class CustomerSummaryModel
    {
        public string Id { get; }

        public string FullName { get; }

        public CustomerSummaryModel(CurrentUserModel currentUserModel)
        {
            Id = currentUserModel.Id;
            FullName = currentUserModel.GetFullName();
        }
     
    }
}
