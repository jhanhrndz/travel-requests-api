using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelRequests.Api.DTOs;
using TravelRequests.Api.Services;

namespace TravelRequests.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var result = await _authService.LoginAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            try
            {
                var result = await _authService.ForgotPasswordAsync(dto);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                var result = await _authService.ResetPasswordAsync(dto);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("users")]
        [Authorize(Roles = "Aprobador")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _authService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}