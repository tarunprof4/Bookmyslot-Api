using Bookmyslot.Api.SlotScheduler.Business;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Business;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Repository;
using Bookmyslot.Api.SlotScheduler.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class SlotSchedulerInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            SlotSchedulerBusinessInjections(services);
            SlotSchedulerRepositoryInjections(services);
        }


        private static void SlotSchedulerBusinessInjections(IServiceCollection services)
        {
            services.AddTransient<ISlotBusiness, SlotBusiness>();
            services.AddTransient<ISlotSchedulerBusiness, SlotSchedulerBusiness>();
            services.AddTransient<ICustomerSharedSlotBusiness, CustomerSharedSlotBusiness>();
            services.AddTransient<ICustomerBookedSlotBusiness, CustomerBookedSlotBusiness>();
            services.AddTransient<ICustomerSlotBusiness, CustomerSlotBusiness>();
            services.AddTransient<IResendSlotInformationBusiness, ResendSlotInformationBusiness>();
            services.AddTransient<ICustomerLastBookedSlotBusiness, CustomerLastBookedSlotBusiness>();
        }


        private static void SlotSchedulerRepositoryInjections(IServiceCollection services)
        {
            services.AddTransient<ISlotRepository, SlotRepository>();
            services.AddTransient<ICustomerSharedSlotRepository, CustomerSharedSlotRepository>();
            services.AddTransient<ICustomerBookedSlotRepository, CustomerBookedSlotRepository>();
            services.AddTransient<ICustomerCancelledSlotRepository, CustomerCancelledSlotRepository>();
            services.AddTransient<ICustomerSlotRepository, CustomerSlotRepository>();
            services.AddTransient<ICustomerLastBookedSlotRepository, CustomerLastBookedSlotRepository>();
        }
    }
}
