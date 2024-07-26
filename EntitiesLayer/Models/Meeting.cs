namespace EntitiesLayer.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public string? DocumentPath { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, StartDate: {StartDate}, EndDate: {EndDate}, Description: {Description}, DocumentPath: {DocumentPath}";
        }
    }
}
