using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using api.Interfaces;
using api.Models;
using api.Data;

namespace api.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return null; // Kullanıcı bulunamadı
            }

            // Şifre doğrulama
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return result == PasswordVerificationResult.Success ? user : null;
        }

        // Kayıt olma metodu (örnek)
        public async Task<bool> RegisterAsync(string firstName, string lastName, string email, string phone, string password, string? profilePicture)
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                ProfilePicture = profilePicture,
                PasswordHash = _passwordHasher.HashPassword(null, password) // Şifreyi hashleyin
            };

            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();

            return result > 0; // Kayıt başarılı ise true döner
        }
    }
}
