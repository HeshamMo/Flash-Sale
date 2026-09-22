using Gateway.Application.Dtos.Authentication;
using Gateway.Application.Services.AuthService;
using Gateway.Application.Services.JwtService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GateWay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }


        [HttpPost("login")]
        public async Task<ActionResult<AuthResultDto>> Login(LogInRequestDto logInRequestDto)
        {
            var logInResult = await _authService.LogInAsync(logInRequestDto);

            if(logInResult.isAuthenticated is not true) { return BadRequest(logInResult); }


            _tokenService.AddRefreshTokenToCookies(logInResult.RefreshToken,
               logInResult.RefreshTokenExpiration, HttpContext);
            return Ok(logInResult);
        }


        [HttpPost("register")]
        public async Task<ActionResult<AuthResultDto>> Register(RegisterRequestDto registerRequestDto)
        {
            var registerationResult = await _authService.RegisterAsync(registerRequestDto);

            if(registerationResult.isAuthenticated is not true) { return BadRequest(registerationResult); }


            _tokenService.AddRefreshTokenToCookies(registerationResult.RefreshToken,
               registerationResult.RefreshTokenExpiration, HttpContext);
            return Ok(registerationResult);
        }


        [Authorize(Roles = "admin,customer")]
        [HttpGet("GetAllClaims")]
        public async Task<IActionResult> getAuthorizedUserInfo()
        {
            var claims = HttpContext.User.Claims.Select(c => new { Type = c.Type, value = c.Value }).ToList();
            return Ok(claims);
        }
    }
}
