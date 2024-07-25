using Microsoft.AspNetCore.Mvc;
using EntitiesLayer.Models;
using BusinessLayer.Interfaces;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using EntitiesLayer.DTOs.Account;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public AuthController(IUserService userService, ITokenService tokenService, IEmailService emailService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        [HttpPost("login")]
        [SwaggerResponse(StatusCodes.Status200OK, "Giriş başarılı.")]
        [SwaggerOperation(Summary = "Giriş işlemi")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.AuthenticateAsync(loginDto.Email, loginDto.Password);

            if (user == null)
            {
                return Unauthorized("Geçersiz kimlik bilgileri");
            }

            var token = _tokenService.GenerateToken(user);

            return Ok(new
            {
                Token = token,
                User = new
                {
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.ProfilePicture
                }
            });
        }

        // [HttpPost("register")]
        // [Consumes("multipart/form-data")]
        // [SwaggerOperation(Summary = "Kullanıcı Kaydı")]
        // [SwaggerResponse(StatusCodes.Status200OK, "Kayıt başarılı.")]
        // [SwaggerResponse(StatusCodes.Status400BadRequest, "Geçersiz e-posta veya diğer kayıt hataları.")]
        // public async Task<IActionResult> Register(
        //     [FromForm] RegisterDto registerDto,
        //     [FromForm] IFormFile profilePicture) //Fromform idi
        // {
        //     if (await _userService.UserExistsAsync(registerDto.Email))
        //     {
        //         return BadRequest("Bu e-posta adresi zaten kayıtlı.");
        //     }

        //     var salt = GenerateSalt();
        //     var hashedPassword = HashPassword(registerDto.Password, salt);

        //     string profilePicturePath = null;
        //     if (profilePicture != null && profilePicture.Length > 0)
        //     {
        //         var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        //         Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

        //         profilePicturePath = Path.Combine(uploadsFolder, profilePicture.FileName);
        //         using (var stream = new FileStream(profilePicturePath, FileMode.Create))
        //         {
        //             await profilePicture.CopyToAsync(stream);
        //         }
        //     }

        //     var newUser = new User
        //     {
        //         FirstName = registerDto.FirstName,
        //         LastName = registerDto.LastName,
        //         Email = registerDto.Email,
        //         Phone = registerDto.Phone,
        //         PasswordHash = hashedPassword,
        //         ProfilePicture = profilePicturePath // Save the path or URL to the profile picture
        //     };

        //     await _userService.CreateUserAsync(newUser);
        //     await _emailService.SendWelcomeEmailAsync(newUser.Email, newUser.FirstName);

        //     return Ok("Kayıt işlemi başarılı.");
        // }

        // private string GenerateSalt()
        // {
        //     byte[] salt = new byte[128 / 8];
        //     using (var rng = RandomNumberGenerator.Create())
        //     {
        //         rng.GetBytes(salt);
        //     }
        //     return Convert.ToBase64String(salt);
        // }

        // private string HashPassword(string password, string salt)
        // {
        //     string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //         password: password,
        //         salt: Convert.FromBase64String(salt),
        //         prf: KeyDerivationPrf.HMACSHA256,
        //         iterationCount: 10000,
        //         numBytesRequested: 256 / 8));

        //     return $"{salt}.{hashed}";
        // }
    }
}
