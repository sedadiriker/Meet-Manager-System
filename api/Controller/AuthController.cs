using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Interfaces;
using System.Threading.Tasks;

namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
{
    var user = await _userService.AuthenticateAsync(loginRequest.Email, loginRequest.Password);
    
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

    }

    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
