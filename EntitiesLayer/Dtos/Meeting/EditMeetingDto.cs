using Microsoft.AspNetCore.Http;

namespace EntitiesLayer.DTOs.Meeting
{
    public class EditMeetingDto
    {
        
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public IFormFile? DocumentPath { get; set; } 
    }
}
