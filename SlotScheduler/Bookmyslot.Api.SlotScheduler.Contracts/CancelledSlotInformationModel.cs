using Bookmyslot.Api.Customers.Contracts;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class CancelledSlotInformationModel
    {
       public CancelledSlotModel CancelledSlotModel { get; set; }

       public CustomerModel CreatedByCustomerModel { get; set; }
    }
}
