using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Honeytor.Models
{
    public class Entry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Dátum Vytvorenia")]
        public DateTime Created { get; set; } = new DateTime();

        [Required]
        [Display(Name = "Dátum vloženia do databázy")]
        public DateTime Stored { get; set; } = new DateTime();

        [Display(Name = "Teplota")]
        public double Temperature { get; set; } = 0.0;

        [Display(Name = "Vlhkosť")]
        public int Hum { get; set; } = 0;

        [Display(Name = "Batéria")]
        public int Bat { get; set; } = 0;

        [Display(Name = "Úľe")]
        [ValidateNever]
        public Hive[] Hives { get; set; } = [];

    }
}
