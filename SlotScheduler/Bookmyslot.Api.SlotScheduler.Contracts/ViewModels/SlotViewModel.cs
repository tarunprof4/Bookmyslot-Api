using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookmyslot.Api.SlotScheduler.Contracts.ViewModels
{
    public class SlotViewModel
    {
        [Required]
        public string Title { get; set; }

        [DefaultValue("Asia/Kolkata")]
        [Required]
        public string TimeZone { get; set; }

        [DefaultValue("13-3-2021")]
        [Required]
        public string SlotDate { get; set; }

        [DefaultValue("10:00:00")]
        [Required]
        public TimeSpan SlotStartTime { get; set; }

        [DefaultValue("11:00:00")]
        [Required]
        public TimeSpan SlotEndTime { get; set; }
    }
}
