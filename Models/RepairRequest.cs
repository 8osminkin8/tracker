namespace Tracker.Models
{
    public enum RepairType
    {
        Electrical,
        Plumbing,
        HVAC,
        General
    }

    public class RepairRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public RepairType Type { get; set; }
        public DateTime RequestDate { get; set; }
    }

}
