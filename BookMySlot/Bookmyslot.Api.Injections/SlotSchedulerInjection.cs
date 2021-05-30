using Bookmyslot.Api.NodaTime.Interfaces;
using Bookmyslot.Api.SlotScheduler.Business;
using Bookmyslot.Api.SlotScheduler.Business.Validations;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Business;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Repository;
using Bookmyslot.Api.SlotScheduler.Repositories;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.RequestAdaptors;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.RequestAdaptors.Interfaces;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using Bookmyslot.Api.SlotScheduler.ViewModels.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmyslot.Api.Injections
{
    public class SlotSchedulerInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            SlotSchedulerViewModelsInjections(services);
            SlotSchedulerBusinessInjections(services);
            SlotSchedulerRepositoryInjections(services);
        }



        private static void SlotSchedulerViewModelsInjections(IServiceCollection services)
        {
            services.AddScoped<IValidator<CancelSlotViewModel>>(x => new CancelSlotViewModelValidator());
            services.AddScoped<IValidator<ResendSlotInformationViewModel>>(x => new ResendSlotInformationViewModelValidator());
            services.AddScoped<IValidator<SlotSchedulerViewModel>>(x => new SlotSchedulerViewModelValidator());
            services.AddScoped<IValidator<SlotViewModel>>(x => new SlotViewModelValidator(x.GetRequiredService<INodaTimeZoneLocationBusiness>()));

            services.AddSingleton<ISlotRequestAdaptor, SlotRequestAdaptor>();
            services.AddSingleton<IAvailableSlotResponseAdaptor, AvailableSlotResponseAdaptor>();
            services.AddSingleton<IBookedSlotResponseAdaptor, BookedSlotResponseAdaptor>();
            services.AddSingleton<ICancelledSlotResponseAdaptor, CancelledSlotResponseAdaptor>();
            services.AddSingleton<ICustomerResponseAdaptor, CustomerResponseAdaptor>();
            services.AddSingleton<ISharedSlotResponseAdaptor, SharedSlotResponseAdaptor>();
        }

        private static void SlotSchedulerBusinessInjections(IServiceCollection services)
        {
            services.AddScoped<IValidator<SlotModel>>(x => new SlotModelValidator());
            

            services.AddTransient<ISlotBusiness, SlotBusiness>();
            services.AddTransient<ISlotSchedulerBusiness, SlotSchedulerBusiness>();
            services.AddTransient<ICustomerSharedSlotBusiness, CustomerSharedSlotBusiness>();
            services.AddTransient<ICustomerBookedSlotBusiness, CustomerBookedSlotBusiness>();
            services.AddTransient<ICustomerSlotBusiness, CustomerSlotBusiness>();
            services.AddTransient<IResendSlotInformationBusiness, ResendSlotInformationBusiness>();
            services.AddTransient<ICustomerLastSharedSlotBusiness, CustomerLastSharedSlotBusiness>();
        }


        private static void SlotSchedulerRepositoryInjections(IServiceCollection services)
        {
            services.AddTransient<ISlotRepository, SlotRepository>();
            services.AddTransient<ICustomerSharedSlotRepository, CustomerSharedSlotRepository>();
            services.AddTransient<ICustomerBookedSlotRepository, CustomerBookedSlotRepository>();
            services.AddTransient<ICustomerCancelledSlotRepository, CustomerCancelledSlotRepository>();
            services.AddTransient<ICustomerSlotRepository, CustomerSlotRepository>();
            services.AddTransient<ICustomerLastSharedSlotRepository, CustomerLastSharedSlotRepository>();
        }
    }
}
