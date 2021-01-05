using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Business;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Bookmyslot.Api.SlotScheduler.Injections
{
    public class SlotSchedulerInjection
    {
        public static void SlotSchedulerBusinessInjections(IServiceCollection services)
        {
            services.AddTransient<ISlotBusiness, SlotBusiness>();
            services.AddTransient<ICustomerSlotBusiness, CustomerSlotBusiness>();
        }


        public static void SlotSchedulerRepositoryInjections(IServiceCollection services, Dictionary<string, string> appConfigurations)
        {
            services.AddTransient<ISlotRepository, SlotRepository>();
            services.AddTransient<ICustomerSlotRepository, CustomerSlotRepository>();
            services.AddTransient<IDbConnection>((sp) => new SqlConnection(appConfigurations[AppConfigurations.BookMySlotDatabaseConnectionString]));
        }
    }
}
