using Gateway.Domain.Entities;
using Gateway.Domain.Entities.Auth;
using Microsoft.AspNetCore.Http;

namespace Gateway.Application.Services.JwtService
{
    public interface ITokenService
    {
        public Task<string> GenerateJwtToken(User user);

        public Task<RefreshToken> GenerateRefreshToken(User user);
        public Task<RefreshToken> RevokeOldAndGenerateNewRefreshToken(string oldRefreshToken, User user);
        public Task<bool> RevokeToken(string refreshToken);

        public Task AddRefreshTokenToCookies(string refreshToken, DateTime refreshTokenExpiration, HttpContext httpContext);
    }
}
