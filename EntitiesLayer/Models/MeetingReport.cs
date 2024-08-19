namespace EntitiesLayer.Models
{
    public class MeetingReport
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public string ReportUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public Meeting Meeting { get; set; } // İlişkilendirme
    }
}
