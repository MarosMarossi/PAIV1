using System.ComponentModel.DataAnnotations;

namespace Honeytor.Models
{
    public class HiveDetails
    {
        public required HiveDevice HiveDevice { get; set; }
        public required List<Entry> Entries { get; set; }
    }
}
