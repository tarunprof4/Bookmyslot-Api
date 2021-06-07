using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Event;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Domain;

namespace Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents
{
    public class SlotCancelledIntegrationEvent : IntegrationEvent
    {
        public string Title { get;  }
        public string Country { get;  }
        public string TimeZone { get;  }
        public string SlotDate { get;  }
        public string SlotStartTime { get;  }
        public string SlotEndTime { get;  }
        public string SlotDuration { get;  }
        public CustomerModel CancelledByCustomerModel { get; }
        public SlotCancelledIntegrationEvent(CancelledSlotModel cancelledSlotModel, CustomerModel cancelledByCustomerModel)
        {
            this.Title = cancelledSlotModel.Title;
            this.Country = cancelledSlotModel.Country;
            this.TimeZone = cancelledSlotModel.SlotStartZonedDateTime.Zone.Id;
            this.SlotDate = NodaTimeHelper.FormatLocalDate(cancelledSlotModel.SlotStartZonedDateTime.Date, DateTimeConstants.ApplicationOutPutDatePattern);
            this.SlotStartTime = cancelledSlotModel.SlotStartTime.ToString();
            this.SlotEndTime = cancelledSlotModel.SlotEndTime.ToString();
            this.SlotDuration = cancelledSlotModel.SlotDuration.TotalMinutes.ToString();
            this.CancelledByCustomerModel = cancelledByCustomerModel;
        }

       
    }
}
