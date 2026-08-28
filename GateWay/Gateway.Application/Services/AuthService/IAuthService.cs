using Gateway.Application.Dtos.Authentication;

namespace Gateway.Application.Services.AuthService
{
    public interface IAuthService
    {

        public Task<AuthResultDto> RegisterAsync(RegisterRequestDto registerRequestDto);
        public Task<AuthResultDto> LogInAsync(LogInRequestDto logInRequestDto);


    }
}
