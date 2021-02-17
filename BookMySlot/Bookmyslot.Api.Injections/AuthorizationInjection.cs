using Bookmyslot.Api.Authorization;
using Bookmyslot.Api.Authorization.Common.Interfaces;
using Bookmyslot.Api.Authorization.Google;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class AuthorizationInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            AppInjections(services);
        }
        private static void AppInjections(IServiceCollection services)
        {
            services.AddSingleton<IJwtTokenProvider, JwtTokenProvider>();
            services.AddSingleton<ISocialLoginTokenValidator, SocialLoginTokenValidator>();
            services.AddSingleton<ITokenValidator, GoogleTokenValidator>();
        }
    }
}
