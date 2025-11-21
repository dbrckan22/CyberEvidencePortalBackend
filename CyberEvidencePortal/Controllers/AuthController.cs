using CyberEvidencePortal.DTOs;
using CyberEvidencePortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace CyberEvidencePortal.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _auth;

        public AuthController(AuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _auth.Login(dto);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new { message = "Login successful", user });
        }
    }
}
