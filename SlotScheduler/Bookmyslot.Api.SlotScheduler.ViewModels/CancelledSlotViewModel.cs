using Bookmyslot.Api.SlotScheduler.Contracts;
using NodaTime;
using System;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class CancelledSlotViewModel
    {
        public string Title { get; set; }

        public string Country { get; set; }
        public ZonedDateTime SlotStartZonedDateTime { get; set; }

        public TimeSpan SlotStartTime { get; set; }

        public TimeSpan SlotEndTime { get; set; }

        public TimeSpan SlotDuration
        {
            get
            {
                return SlotEndTime - SlotStartTime;
            }
        }

        public static CancelledSlotViewModel CreateCancelledSlotViewModel(CancelledSlotModel cancelledSlotModel)
        {
            var cancelledSlotViewModel = new CancelledSlotViewModel
            {
                Title = cancelledSlotModel.Title,
                Country = cancelledSlotModel.Country,
                SlotStartZonedDateTime = cancelledSlotModel.SlotStartZonedDateTime,
                SlotStartTime = cancelledSlotModel.SlotStartTime,
                SlotEndTime = cancelledSlotModel.SlotEndTime,
            };

            return cancelledSlotViewModel;
        }

        public static IEnumerable<CancelledSlotViewModel> CreateCancelledSlotViewModels(IEnumerable<CancelledSlotModel> cancelledSlotModels)
        {
            var cancelledSlotViewModels = new List<CancelledSlotViewModel>();

            foreach (var cancelledSlotModel in cancelledSlotModels)
            {
                cancelledSlotViewModels.Add(CreateCancelledSlotViewModel(cancelledSlotModel));
            }

            return cancelledSlotViewModels;
        }
    }
}
