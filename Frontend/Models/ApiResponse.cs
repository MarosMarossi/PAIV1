namespace Honeytor.Models
{
    public class ApiResponse
    {
        public int Device { get; set; } = 0;
        public string From { get; set; } = "";
        public List<Entry> HiveFiles { get; set; } = [];
    }
}
