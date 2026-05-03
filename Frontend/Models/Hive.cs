using System.ComponentModel.DataAnnotations;

namespace Honeytor.Models
{
    public class Hive
    {
        [Required]
        public string HiveId { get; set; } = "1.?";

        [Required]
        public string Id { get; set; } = "1.?";

        [Display(Name = "Váha")] /* ide brutal ide hore */
        public double Weight { get; set; } = -1;

        [Display(Name = "Teplota")]
        public double Temperature { get; set; } = -1;

        [Display(Name = "Vlhkosť")]
        public int Humidity { get; set; } = -1;
    }
}
