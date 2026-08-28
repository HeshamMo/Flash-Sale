using Gateway.Application.Dtos.Authentication;
using Gateway.Application.Services.JwtService;
using Gateway.Domain.Constants;
using Gateway.Domain.Entities;
using Gateway.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;

namespace Gateway.Application.Services.AuthService
{
    public class AuthService:IAuthService
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ITokenService _tokenService;
        public AuthService(UserManager<User> userManager, RoleManager<Role> roleManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResultDto> LogInAsync(LogInRequestDto logInRequestDto)
        {


            var user = await _userManager.FindByEmailAsync(logInRequestDto.Email);
            if(user is null) { return new AuthResultDto() { Message = "User Does Not Exist" }; }


            var IsAuthenticated = await _userManager.CheckPasswordAsync(user, logInRequestDto.Password);
            if(IsAuthenticated is not true) { return new AuthResultDto() { Message = "Wrong Email or Password!" }; }

            var roles = (await _userManager.GetRolesAsync(user)).Select(r => r.ToString().ToLower());

            var JwtToken = await _tokenService.GenerateJwtToken(user);
            var RefreshToken = await _tokenService.GenerateRefreshToken(user);

            if(RefreshToken is null || JwtToken is null) { return new AuthResultDto() { Message = "Something worng occured!" }; }

            return new AuthResultDto()
            {
                Id = user.Id,
                isAuthenticated = IsAuthenticated,
                Token = JwtToken,
                RefreshToken = RefreshToken.Token,
                RefreshTokenExpiration = RefreshToken.ExpiresOn,
                roles = roles,
            };

        }

        public async Task<AuthResultDto> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerRequestDto.Email);
            if(existingUser is not null)
            { return new AuthResultDto() { Message = "Email Already Exists!" }; }

            var user = new User()
            {
                Email = registerRequestDto.Email,
                UserName = registerRequestDto.UserName,
            };

            var userCreationResult = await _userManager.CreateAsync(user, registerRequestDto.Password);
            if(userCreationResult.Succeeded is not true) { return new AuthResultDto() { Message = "An Error Occured" }; }


            var addRoleToUserResult = await _userManager.AddToRoleAsync(user, RolesConstants.NewRegistered);
            if(addRoleToUserResult.Succeeded is not true) { return new AuthResultDto() { Message = "An Error Occured" }; }


            var JwtToken = await _tokenService.GenerateJwtToken(user);
            var RefreshToken = await _tokenService.GenerateRefreshToken(user);

            if(RefreshToken is null || JwtToken is null) { return new AuthResultDto() { Message = "Something worng occured!" }; }

            return new AuthResultDto()
            {
                Id = user.Id,
                isAuthenticated = true,
                Token = JwtToken,
                RefreshToken = RefreshToken.Token,
                RefreshTokenExpiration = RefreshToken.ExpiresOn,
                roles = new List<string>() { RolesConstants.NewRegistered },
            };
        }
    }
}
