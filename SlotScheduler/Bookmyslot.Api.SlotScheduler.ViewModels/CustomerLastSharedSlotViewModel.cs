using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class CustomerLastSharedSlotViewModel
    {
        public string Title { get; set; }
        public string Country { get; set; }

        public string TimeZone { get; set; }

        public ZonedDateTime SlotStartZonedDateTime { get; set; }

        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan SlotStartTime { get; set; }

        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan SlotEndTime { get; set; }

        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan SlotDuration
        {
            get
            {
                return SlotEndTime - SlotStartTime;
            }
        }


        public static CustomerLastSharedSlotViewModel CreateCurrentUserViewModel(CustomerLastSharedSlotModel CustomerLastSharedSlotModel)
        {
            var customerLastSharedSlotViewModel = new CustomerLastSharedSlotViewModel
            {
                Title = CustomerLastSharedSlotModel.Title,
                Country = CustomerLastSharedSlotModel.Country,
                TimeZone = CustomerLastSharedSlotModel.TimeZone,
                SlotStartZonedDateTime = CustomerLastSharedSlotModel.SlotStartZonedDateTime,
                SlotStartTime = CustomerLastSharedSlotModel.SlotStartTime,
                SlotEndTime = CustomerLastSharedSlotModel.SlotEndTime,
            };

            return customerLastSharedSlotViewModel;
        }
    }
}
