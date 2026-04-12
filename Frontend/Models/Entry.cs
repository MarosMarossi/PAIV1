using System.ComponentModel.DataAnnotations;

namespace Honeytor.Models
{
    public class Entry
    {
        [Required]
        public int EntryId { get; set; }

        [Display(Name = "Vlhkosť")]
        public double Humidity { get; set; } = 0.0;

        [Display(Name = "Váha")]
        public double Weight { get; set; } = 0.0;

        [Display(Name = "Teplota")]
        public double Temperature { get; set; } = 0.0;

        [Display(Name = "Dátum merania")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
