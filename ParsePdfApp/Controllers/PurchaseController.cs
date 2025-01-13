using ParsePdfApp.Services;
using Microsoft.AspNetCore.Mvc;
using ParsePdfApp.Dtos;
using ParsePdfApp.Utils;

namespace ParsePdfApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PurchaseController : Controller
    {
        private readonly PurchaseService _purchaseService;

        public PurchaseController(PurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<AuthUserDto>> SignupUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AuthUserDto result = await _userService.SignupUser(userDto);

            SetTokenCookies(result.RefreshToken);

            return Ok(result);
        }

        [HttpPost("signin")]
        public async Task<ActionResult<AuthUserDto>> SinginUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AuthUserDto result = await _userService.SigninUser(userDto);

            SetTokenCookies(result.RefreshToken);

            return Ok(result);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("RefreshToken");

            return Ok(new { Message = "User logged out successfully" });
        }

        [HttpGet("refresh")]
        public async Task<ActionResult<AuthUserDto>> RefreshUser()
        {
            string? refreshToken = Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Refresh token not found. Please login again.");
            }

            AuthUserDto result = await _userService.RefreshUser(refreshToken);

            SetTokenCookies(result.RefreshToken);

            return Ok(result);
        }

        private void SetTokenCookies(string refreshToken)
        {
            Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(30)
            });
        }

    }
}
