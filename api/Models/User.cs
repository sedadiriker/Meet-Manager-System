namespace api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? PasswordHash { get; set; } // PasswordHash olarak değiştirin
        public string? ProfilePicture { get; set; }
    }
}
