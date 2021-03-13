﻿using Bookmyslot.Api.Common.Database.Constants;

namespace Bookmyslot.Api.Customers.Repositories.Queries
{

    public class CustomerTableQueries
    {
        public const string UpdateProfileSettingQuery = @"UPDATE" + " " + TableNameConstants.RegisterCustomer + " " + @"SET 
 FirstName = @FirstName,  LastName = @LastName, Gender=@Gender, ModifiedDateUtc =@ModifiedDateUtc
 WHERE UniqueId=@customerId";

        public const string GetCustomersByCustomerIdsQuery = @"select UniqueId, Email, FirstName, LastName, BioHeadLine from" + " " + TableNameConstants.RegisterCustomer + " " + "where UniqueId IN @CustomerIds";

        public const string GetCustomerByEmailQuery = @"select UniqueId, Email, FirstName, LastName, BioHeadLine from" + " " + TableNameConstants.RegisterCustomer + " " + "where Email = @Email";

        public const string GetCustomerIdByEmailQuery = @"select UniqueId from" + " " + TableNameConstants.RegisterCustomer + " " + "where Email = @Email";

        public const string GetCustomerByIdQuery = @"select UniqueId, Email, FirstName, LastName, BioHeadLine from" + " " + TableNameConstants.RegisterCustomer + " " + "where UniqueId = @CustomerId";

        public const string GetProfileSettingsByCustomerIdQuery = @"select Email, FirstName, LastName, Gender from" + " " + TableNameConstants.RegisterCustomer + " " + "where UniqueId=@customerId";

        public const string GetCustomerAdditionInformationQuery = @"select timeZone from" + " " + TableNameConstants.CustomerAdditionalInformation + " " + "where CustomerId=@customerId";


        public const string InsertOrUpdateCustomerAdditionInformationQuery =
            @"UPDATE" + " " + TableNameConstants.CustomerAdditionalInformation + " " + @"set TimeZone=@timeZone, ModifiedDateUtc=@ModifiedDateUtc where CustomerId=@customerId
            IF @@ROWCOUNT=0
            BEGIN
            INSERT INTO CustomerAdditionalInformation(TimeZone,ModifiedDateUtc, CustomerId )
            VALUES
            (@timeZone, @ModifiedDateUtc, @customerId)
            END";

    }
}
