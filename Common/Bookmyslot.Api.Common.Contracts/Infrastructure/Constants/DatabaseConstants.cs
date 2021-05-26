namespace Bookmyslot.Api.Common.Contracts.Infrastructure.Database.Constants
{
    public class DatabaseConstants
    {
        public const string BookmyslotDatabase = "bookmyslot.";

        public const string CacheTable = "Cache";


        public const string RegisterCustomerTable = BookmyslotDatabase + "registercustomer";
        public const string SlotTable = BookmyslotDatabase + "slot";
        public const string CancelledSlotTable = BookmyslotDatabase + "cancelledslot";
        public const string CustomerSettingsTable = BookmyslotDatabase + "customersettings";

        public const string CustomerLastSharedSlotTable = BookmyslotDatabase + "customerlastsharedslot";

        public const string SearchTable = BookmyslotDatabase + "search";
    }
}
