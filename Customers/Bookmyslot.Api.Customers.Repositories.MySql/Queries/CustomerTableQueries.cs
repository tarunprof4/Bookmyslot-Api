﻿using Bookmyslot.Api.Common.Database.Constants;

namespace Bookmyslot.Api.Customers.Repositories.Queries
{

    public class CustomerTableQueries
    {

        public const string RegisterCustomerQuery = @"INSERT INTO" + " " + DatabaseConstants.RegisterCustomerTable + " " + @"(Id, FirstName, LastName, UserName, Email, Provider, PhotoUrl, CreatedDateUtc, IsVerified)
VALUES(@Id, @FirstName, @LastName, @UserName, @Email, @Provider, @PhotoUrl, @CreatedDateUtc, @IsVerified); ";


        public const string UpdateProfileSettingQuery = @"UPDATE" + " " + DatabaseConstants.RegisterCustomerTable + " " + @"SET 
 FirstName = @FirstName,  LastName = @LastName, Gender=@Gender, ModifiedDateUtc =@ModifiedDateUtc
 WHERE Id=@customerId";

        public const string UpdateProfilePictureQuery = @"UPDATE" + " " + DatabaseConstants.RegisterCustomerTable + " " + @"SET 
   PhotoUrl=@profilePictureUrl, ModifiedDateUtc =@modifiedDateUtc WHERE Id=@customerId";

        public const string GetCustomersByCustomerIdsQuery = @"select Id, Email, FirstName, LastName, BioHeadLine, IsVerified from" + " " + DatabaseConstants.RegisterCustomerTable + " " + "where Id IN @CustomerIds";

        public const string GetCustomerByEmailQuery = @"select Id, Email, FirstName, LastName, BioHeadLine, IsVerified from" + " " + DatabaseConstants.RegisterCustomerTable + " " + "where Email = @Email";

        public const string GetCustomerIdByEmailQuery = @"select Id from" + " " + DatabaseConstants.RegisterCustomerTable + " " + "where Email = @Email";

        public const string GetCustomerByIdQuery = @"select Id, Email, FirstName, LastName, BioHeadLine, IsVerified from" + " " + DatabaseConstants.RegisterCustomerTable + " " + "where Id = @CustomerId";

        public const string GetProfileSettingsByCustomerIdQuery = @"select Email, FirstName, LastName, Gender from" + " " + DatabaseConstants.RegisterCustomerTable + " " + "where Id=@customerId";

        public const string GetCustomerSettingsQuery = @"select * from" + " " + DatabaseConstants.CustomerSettingsTable + " " + "where CustomerId=@customerId";


        public const string InsertOrUpdateCustomerSettingsQuery = @"INSERT INTO" + " " + DatabaseConstants.CustomerSettingsTable + " " + @"(CustomerId,Country, TimeZone,  ModifiedDateUtc) VALUES(@customerId, @Country, @timeZone, @ModifiedDateUtc) ON DUPLICATE KEY UPDATE
 Country=@Country, TimeZone=@timeZone,  ModifiedDateUtc = @ModifiedDateUtc";

    }
}
