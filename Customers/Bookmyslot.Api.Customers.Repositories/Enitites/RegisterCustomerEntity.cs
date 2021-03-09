﻿using Bookmyslot.Api.Common.Database.Constants;
using Dapper;
using System;

namespace Bookmyslot.Api.Customers.Repositories.Enitites
{
    [Table(TableNameConstants.Register)]
    public class RegisterCustomerEntity
    {
        [Key]
        public string UniqueId { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string BioHeadLine { get; set; }

        public string Provider { get; set; }

        public string PhotoUrl { get; set; }

        public DateTime CreatedDateUtc { get; set; }
        public DateTime? ModifiedDateUtc { get; set; }
    }
}
