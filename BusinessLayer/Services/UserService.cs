using Microsoft.EntityFrameworkCore;
using BusinessLayer.Interfaces;
using EntitiesLayer.Models;
using DataAccessLayer.Data;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            
            if (user == null)
            {
                return null;
            }

            // Şifre doğrulama
            var hashedPassword = HashPassword(password, user.PasswordHash.Split('.')[0]);
            if (user.PasswordHash != hashedPassword)
            {
                return null;
            }

            return user;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        private string HashPassword(string password, string salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{salt}.{hashed}";
        }
    }
}
