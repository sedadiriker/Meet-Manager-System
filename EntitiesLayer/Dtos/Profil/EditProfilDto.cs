using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EntitiesLayer.DTOs.Account
{
    public class EditProfilDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Password { get; set; }

        public IFormFile ProfilePicture { get; set; }  // Dosya yükleme alanı
    }
}
