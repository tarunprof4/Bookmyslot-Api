using Bookmyslot.Api.Common.Web.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{


    public class WebInjections
    {
        public static void LoadInjections(IServiceCollection services)
        {
            services.AddScoped<AuthorizedFilter>();
        }
  
    }
}
