using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bookmyslot.Api.SlotScheduler.Contracts
{
    public class SlotModel
    {
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid Id { get; set; }
        
        [Required]
        public string Title { get; set; }

        [JsonIgnore]
        public string CreatedBy { get; set; }

        [DefaultValue("India Standard Time")]
        [Required]
        public string TimeZone { get; set; }

        [Required]
        public DateTime SlotDate { get; set; }

        [JsonConverter(typeof(JsonTimeSpanConverter))]
        [DefaultValue("10:00:00")]
        [Required]
        public TimeSpan SlotStartTime { get; set; }

        [JsonConverter(typeof(JsonTimeSpanConverter))]
        [DefaultValue("11:00:00")]
        [Required]
        public TimeSpan SlotEndTime { get; set; }

        [JsonIgnore]
        public TimeSpan SlotDuration
        {
            get
            {
                return SlotEndTime - SlotStartTime;
            }
        }
    }
}
