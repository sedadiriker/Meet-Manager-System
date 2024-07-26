public class Meeting
{
    public int Id { get; set; }
    public string? Name { get; set; } // Nullable string
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Description { get; set; } // Nullable string
    public string? DocumentPath { get; set; } // Nullable string

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, StartDate: {StartDate}, EndDate: {EndDate}, Description: {Description}, DocumentPath: {DocumentPath}";
    }
}
