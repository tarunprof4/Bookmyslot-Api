using Bookmyslot.Api.Authentication;
using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Authentication.Facebook;
using Bookmyslot.Api.Authentication.Google;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class AuthenticationInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            AppInjections(services);
        }
        private static void AppInjections(IServiceCollection services)
        {
            services.AddSingleton<IJwtTokenProvider, JwtTokenProvider>();
            services.AddSingleton<ISocialLoginTokenValidator, SocialLoginTokenValidator>();
            services.AddSingleton<IGoogleTokenValidator, GoogleTokenValidator>();
            services.AddTransient<IFacebookTokenValidator, FaceBookTokenValidator>();
            services.AddTransient<ICurrentUser, CurrentUser>();
        }
    }
}
