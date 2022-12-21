using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Authentication.Common.Constants;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.ValueObject;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business
{
    public class LoginCustomerBusiness : ILoginCustomerBusiness
    {
        private readonly IRegisterCustomerBusiness registerCustomerBusiness;
        private readonly ICustomerBusiness customerBusiness;
        private readonly ISocialLoginTokenValidator socialLoginTokenValidator;
        private readonly IJwtTokenProvider jwtTokenProvider;
        private readonly ICurrentUser currentUser;
        private readonly IValidator<SocialCustomerLoginModel> socialLoginCustomerValidator;

        public LoginCustomerBusiness(IRegisterCustomerBusiness registerCustomerBusiness, ICustomerBusiness customerBusiness, ISocialLoginTokenValidator socialLoginTokenValidator, IJwtTokenProvider jwtTokenProvider, ICurrentUser currentUser, IValidator<SocialCustomerLoginModel> socialLoginCustomerValidator)
        {
            this.registerCustomerBusiness = registerCustomerBusiness;
            this.customerBusiness = customerBusiness;
            this.socialLoginTokenValidator = socialLoginTokenValidator;
            this.jwtTokenProvider = jwtTokenProvider;
            this.currentUser = currentUser;
            this.socialLoginCustomerValidator = socialLoginCustomerValidator;
        }

        private void SanitizeSocialCustomerLoginModel(SocialCustomerLoginModel socialCustomerLoginModel)
        {
            socialCustomerLoginModel.Provider = socialCustomerLoginModel.Provider.Trim().ToLowerInvariant();
        }

        public async Task<Result<string>> LoginSocialCustomer(SocialCustomerLoginModel socialCustomerLoginModel)
        {
            ValidationResult results = this.socialLoginCustomerValidator.Validate(socialCustomerLoginModel);

            if (results.IsValid)
            {
                SanitizeSocialCustomerLoginModel(socialCustomerLoginModel);
                var validateTokenResponse = await ValidateSocialCustomerToken(socialCustomerLoginModel);
                if (validateTokenResponse.ResultType == ResultType.Success)
                {
                    var validatedSocialCustomer = validateTokenResponse.Value;

                    var tokenResponse = await AllowLoginOrRegistration(validatedSocialCustomer);
                    if (tokenResponse.ResultType == ResultType.Success)
                    {
                        await this.currentUser.SetCurrentUserInCache(validatedSocialCustomer.Email);
                    }
                    return tokenResponse;
                }

                return new Result<string>() { ResultType = ResultType.ValidationError, Messages = validateTokenResponse.Messages };
            }

            return new Result<string>() { ResultType = ResultType.ValidationError, Messages = results.Errors.Select(a => a.ErrorMessage).ToList() };
        }

        private async Task<Result<SocialCustomerModel>> ValidateSocialCustomerToken(SocialCustomerLoginModel socialCustomerLoginModel)
        {
            if (socialCustomerLoginModel.Provider == LoginConstants.ProviderGoogle)
            {
                return await this.socialLoginTokenValidator.LoginWithGoogle(socialCustomerLoginModel.IdToken);
            }

            else if (socialCustomerLoginModel.Provider == LoginConstants.ProviderFacebook)
            {
                return await this.socialLoginTokenValidator.LoginWithFacebook(socialCustomerLoginModel.AuthToken);
            }

            return new Result<SocialCustomerModel>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { AppBusinessMessagesConstants.InValidTokenProvider } };
        }


        private async Task<Result<string>> AllowLoginOrRegistration(SocialCustomerModel socialCustomerModel)
        {
            var checkIfCustomerExistsResponse = CheckIfCustomerExists(socialCustomerModel.Email);
            if (checkIfCustomerExistsResponse.Result)
            {
                return CreateJwtToken(socialCustomerModel.Email);
            }

            var registerCustomerModel = CreateRegisterCustomerModel(socialCustomerModel);
            registerCustomerModel.RegisterCustomer();

            var registerCustomerResponse = await registerCustomerBusiness.RegisterCustomer(registerCustomerModel);
            if (registerCustomerResponse.ResultType == ResultType.Success)
            {
                return CreateJwtToken(registerCustomerModel.Email);
            }

            return Result<string>.ValidationError(new List<string>() { AppBusinessMessagesConstants.LoginFailed });
        }

        private Result<string> CreateJwtToken(string email)
        {
            var jwtToken = this.jwtTokenProvider.GenerateToken(email);
            return new Result<string>() { Value = jwtToken };
        }

        private RegisterCustomerModel CreateRegisterCustomerModel(SocialCustomerModel socialCustomer)
        {
            return new RegisterCustomerModel()
            {
                FirstName = socialCustomer.FirstName,
                LastName = socialCustomer.LastName,
                Email = socialCustomer.Email,
                Provider = socialCustomer.Provider
            };
        }


        private async Task<bool> CheckIfCustomerExists(string email)
        {
            var customerResponse = await this.customerBusiness.GetCustomerByEmail(email);
            if (customerResponse.ResultType == ResultType.Success)
            {
                return true;
            }

            return false;
        }


    }
}
