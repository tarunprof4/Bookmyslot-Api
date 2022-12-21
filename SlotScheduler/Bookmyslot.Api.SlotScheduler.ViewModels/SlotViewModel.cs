using Bookmyslot.SharedKernel.Constants;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class SlotViewModel
    {
        [Required]
        public string Title { get; set; }

        [DefaultValue(CountryConstants.India)]
        [Required]
        public string Country { get; set; }

        [DefaultValue(TimeZoneConstants.IndianTimezone)]
        [Required]
        public string TimeZone { get; set; }

        [DefaultValue("03-10-2021")]
        [Required]
        public string SlotDate { get; set; }

        [DefaultValue("10:00:00")]
        [Required]
        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan SlotStartTime { get; set; }

        [DefaultValue("11:00:00")]
        [Required]
        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan SlotEndTime { get; set; }
    }
}
