﻿using Bookmyslot.Api.Common.Database.Constants;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookmyslot.Api.Customers.Repositories.Enitites
{
    [Table(DatabaseConstants.CustomerSettingsTable)]
    public class CustomerSettingsEntity
    {
        public string CustomerId { get; set; }

        public string Country { get; set; }
        public string TimeZone { get; set; }

        public DateTime ModifiedDateUtc { get; set; }
    }
}
