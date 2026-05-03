using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Honeytor.Models
{
    public class HiveDevice
    {
        [Required]
        public int HiveDeviceId { get; set; }

        [Required]
        [Display(Name = "ID zariadenia")]
        public int DeviceReference { get; set; } = 0;

        [Required]
        [MinLength(4)]
        [MaxLength(4)]
        public string PIN { get; set; } = "69AE";

        [Required]
        [Display(Name = "Záznamy od")]
        public DateTime TimeFrom { get; set; } = new DateTime(2020, 7, 24, new JulianCalendar());
    }
}
