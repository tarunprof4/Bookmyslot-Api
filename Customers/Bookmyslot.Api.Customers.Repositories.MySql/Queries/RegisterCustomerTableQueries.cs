using Bookmyslot.Api.Common.Contracts.Infrastructure.Database.Constants;

namespace Bookmyslot.Api.Customers.Repositories.Queries
{

    public class RegisterCustomerTableQueries
    {

        public const string RegisterCustomerQuery = @"INSERT INTO" + " " + DatabaseConstants.RegisterCustomerTable + " " + @"(Id, FirstName, LastName, UserName, Email, Provider, CreatedDateUtc, IsVerified)
VALUES(@Id, @FirstName, @LastName, @UserName, @Email, @Provider, @CreatedDateUtc, @IsVerified); ";


        public const string UpdateProfileSettingQuery = @"UPDATE" + " " + DatabaseConstants.RegisterCustomerTable + " " + @"SET 
 FirstName = @FirstName,  LastName = @LastName, Gender=@Gender, ModifiedDateUtc =@ModifiedDateUtc
 WHERE Id=@customerId";

        public const string UpdateProfilePictureQuery = @"UPDATE" + " " + DatabaseConstants.RegisterCustomerTable + " " + @"SET 
   ProfilePictureUrl=@profilePictureUrl, ModifiedDateUtc =@modifiedDateUtc WHERE Id=@customerId";

        public const string GetCustomersByCustomerIdsQuery = @"select Id, Email, FirstName, LastName, BioHeadLine, IsVerified, ProfilePictureUrl, UserName from" + " " + DatabaseConstants.RegisterCustomerTable + " " + "where Id IN @CustomerIds";

        public const string GetCustomerByEmailQuery = @"select Id, Email, FirstName, LastName, BioHeadLine, IsVerified, ProfilePictureUrl, UserName from" + " " + DatabaseConstants.RegisterCustomerTable + " " + "where Email = @Email";

        public const string GetCustomerIdByEmailQuery = @"select Id from" + " " + DatabaseConstants.RegisterCustomerTable + " " + "where Email = @Email";

        public const string GetCustomerByIdQuery = @"select Id, Email, FirstName, LastName, BioHeadLine, IsVerified, ProfilePictureUrl, UserName from" + " " + DatabaseConstants.RegisterCustomerTable + " " + "where Id = @CustomerId";

        public const string GetProfileSettingsByCustomerIdQuery = @"select Email, FirstName, LastName, Gender from" + " " + DatabaseConstants.RegisterCustomerTable + " " + "where Id=@customerId";



        public const string GetAdditionalProfileSettingsByCustomerIdQuery = @"select BioHeadLine from" + " " + DatabaseConstants.RegisterCustomerTable + " " + "where Id=@customerId";


        public const string UpdateAdditionalProfileSettingQuery = @"UPDATE" + " " + DatabaseConstants.RegisterCustomerTable + " " + @"SET 
    BioHeadLine = @bioHeadLine,  ModifiedDateUtc =@modifiedDateUtc WHERE Id=@customerId";

    }
}
