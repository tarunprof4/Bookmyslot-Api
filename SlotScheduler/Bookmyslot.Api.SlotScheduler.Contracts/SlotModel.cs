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

        public string CreatedBy { get; set; }
        
        public string BookedBy { get; set; }

        [DefaultValue("India Standard Time")]
        [Required]
        public string TimeZone { get; set; }

        [DefaultValue("13-3-2021")]
        [Required]
        public string SlotDate { get; set; }
       

        [JsonIgnore]
        public DateTime SlotDateUtc { get; set; }
       

        [JsonConverter(typeof(JsonTimeSpanConverter))]
        [DefaultValue("10:00:00")]
        [Required]
        public TimeSpan SlotStartTime { get; set; }

        [JsonConverter(typeof(JsonTimeSpanConverter))]
        [DefaultValue("11:00:00")]
        [Required]
        public TimeSpan SlotEndTime { get; set; }

        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan SlotDuration
        {
            get
            {
                return SlotEndTime - SlotStartTime;
            }
        }

        public DateTime CreatedDateUtc { get; set; }
    }
}
