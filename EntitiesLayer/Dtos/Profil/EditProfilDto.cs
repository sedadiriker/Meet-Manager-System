using Microsoft.AspNetCore.Http;

namespace EntitiesLayer.DTOs.Account
{
    public class UpdateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; } 
        public IFormFile? ProfilePicture { get; set; }
    }
}
