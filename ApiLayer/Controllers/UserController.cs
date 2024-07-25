using Microsoft.AspNetCore.Mvc;
using EntitiesLayer.Models;
using BusinessLayer.Interfaces;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EntitiesLayer.DTOs.Account;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        // Get users
        [HttpGet]
        [SwaggerOperation(Summary = "Tüm kullanıcıları listele")]
        [SwaggerResponse(StatusCodes.Status200OK, "Kullanıcılar başarılı şekilde listelendi.")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Get user
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Belirli bir kullanıcıyı getir")]
        [SwaggerResponse(StatusCodes.Status200OK, "Kullanıcı başarılı şekilde getirildi.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Kullanıcı bulunamadı.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "E-posta veya Şifre Hatası.")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }
            return Ok(user);
        }

        // POST user
        [HttpPost]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(Summary = "Yeni kullanıcı oluştur")]
        [SwaggerResponse(StatusCodes.Status200OK, "Kullanıcı başarılı şekilde oluşturuldu.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Geçersiz e-posta veya diğer kayıt hataları.")]
        public async Task<IActionResult> CreateUser(
            [FromForm] RegisterDto registerDto,
            [FromForm] IFormFile profilePicture)
        {
            if (await _userService.UserExistsAsync(registerDto.Email))
            {
                return BadRequest("Bu e-posta adresi zaten kayıtlı.");
            }

            var salt = GenerateSalt();
            var hashedPassword = HashPassword(registerDto.Password, salt);

            string profilePicturePath = null;
            if (profilePicture != null && profilePicture.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder); 

                profilePicturePath = Path.Combine(uploadsFolder, profilePicture.FileName);
                using (var stream = new FileStream(profilePicturePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }
            }

            var newUser = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Phone = registerDto.Phone,
                PasswordHash = hashedPassword,
                ProfilePicture = profilePicturePath
            };

            await _userService.CreateUserAsync(newUser);
            await _emailService.SendWelcomeEmailAsync(newUser.Email, newUser.FirstName);

            return Ok("Kayıt işlemi başarılı.");
        }
        // PUT User
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(Summary = "Kullanıcıyı güncelle")]
        [SwaggerResponse(StatusCodes.Status200OK, "Kullanıcı başarılı şekilde güncellendi.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Kullanıcı bulunamadı.")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] UpdateUserDto updatedUserDto, [FromForm] IFormFile profilePicture)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            user.FirstName = updatedUserDto.FirstName;
            user.LastName = updatedUserDto.LastName;
            user.Email = updatedUserDto.Email;
            user.Phone = updatedUserDto.Phone;

            if (!string.IsNullOrEmpty(updatedUserDto.Password))
            {
                var salt = GenerateSalt();
                var hashedPassword = HashPassword(updatedUserDto.Password, salt);
                user.PasswordHash = hashedPassword;
            }

            if (profilePicture != null && profilePicture.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

                var profilePicturePath = Path.Combine(uploadsFolder, profilePicture.FileName);
                using (var stream = new FileStream(profilePicturePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }

                user.ProfilePicture = profilePicturePath;
            }

            await _userService.UpdateUserAsync(user);
            return Ok("Kullanıcı başarılı şekilde güncellendi.");
        }



        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Kullanıcıyı sil")]
        [SwaggerResponse(StatusCodes.Status200OK, "Kullanıcı başarılı şekilde silindi.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Kullanıcı bulunamadı.")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            await _userService.DeleteUserAsync(id);
            return Ok("Kullanıcı başarılı şekilde silindi.");
        }

        private string GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
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
