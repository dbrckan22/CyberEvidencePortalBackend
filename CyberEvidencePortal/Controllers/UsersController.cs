using CyberEvidencePortal.DTOs;
using CyberEvidencePortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace CyberEvidencePortal.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register/user")]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(dto);
                return Ok(new { message = "User created", user });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
