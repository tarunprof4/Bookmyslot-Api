using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Injections
{
    public class HttpFactoryInjections
    {
        public static void LoadInjections(IServiceCollection services)
        {
            services.AddHttpClient();
        }
    }
}
