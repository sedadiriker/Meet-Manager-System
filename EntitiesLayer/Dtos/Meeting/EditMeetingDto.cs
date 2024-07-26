using Microsoft.AspNetCore.Http;

namespace EntitiesLayer.DTOs.Meeting
{
    public class UpdateMeetingDto
    {
        
        public string? Namee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public IFormFile? DocumentPath { get; set; } 
    }
}
