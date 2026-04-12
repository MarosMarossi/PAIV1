using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Honeytor.Models
{
    public class Microcontroller
    {
        [Required]
        public int MicrocontrollerId { get; set; }

        [ValidateNever]
        public virtual ICollection<Entry>? Entries { get; set; } = null;
    }
}
