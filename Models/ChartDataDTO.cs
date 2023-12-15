namespace Tracker.Models
{
    public class ChartDataDTO
    {
        public string[] Dates { get; set; }
        public Dictionary<string, int[]> CountsByType { get; set; }
    }
}
