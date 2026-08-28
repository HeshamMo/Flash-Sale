using Gateway.Application.Options;
using Gateway.Domain;
using Gateway.Domain.Contants;
using Gateway.Domain.Entities;
using Gateway.Domain.Entities.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gateway.Application.Services.JwtService
{
    public class TokenService:ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly JwtOptions _jwtOptions;
        private readonly TimeProvider _timeProvider;
        private readonly ApplicationDbContext _dbContext;

        public TokenService(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOptions<JwtOptions> options, TimeProvider timeProvider
, ApplicationDbContext dbContext


            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtOptions = options.Value;
            _timeProvider = timeProvider;
            _dbContext = dbContext;
        }



        public async Task<string> GenerateJwtToken(User user)
        {

            var roles = await _userManager.GetRolesAsync(user);
            List<Claim> userClaims = new List<Claim>();
            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            userClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
            userClaims.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach(var role in roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
          issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: userClaims,
            expires: DateTime.UtcNow.AddMinutes(
                _jwtOptions.ExpirationInMinutes),
            signingCredentials: credentials
                        );

            return new JwtSecurityTokenHandler()
          .WriteToken(token);
        }

        public async Task<RefreshToken> GenerateRefreshToken(User user)
        {
            var refreshToken = new RefreshToken()
            {
                CreatedoOn = _timeProvider.GetUtcNow().UtcDateTime,
                ExpiresOn = _timeProvider.GetUtcNow().UtcDateTime.AddDays
                (_jwtOptions.RefreshTokenExpirationInDays),
                UserId = user.Id,
                Token = Guid.NewGuid().ToString(),
            };

            user.RefreshTokens.Add(refreshToken);
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded ? refreshToken : null;

        }

        public async Task<RefreshToken> RevokeOldAndGenerateNewRefreshToken(string oldRefreshToken, User user)
        {
            var refreshToken = await _dbContext.RefreshTokens.SingleOrDefaultAsync(t => t.Token == oldRefreshToken);
            if(refreshToken is null || refreshToken.UserId != user.Id) { return null; }


            refreshToken.RevokedOn = _timeProvider.GetUtcNow().UtcDateTime;
            _dbContext.Update(refreshToken);


            int updateAffectedEntitiesCount = await _dbContext.SaveChangesAsync();
            if(updateAffectedEntitiesCount < 1) { return null; }


            return await this.GenerateRefreshToken(user);



        }

        public async Task<bool> RevokeToken(string oldRefreshToken)
        {
            var refreshToken = await _dbContext.RefreshTokens.SingleOrDefaultAsync(t => t.Token == oldRefreshToken);
            if(refreshToken is null) { return false; }

            refreshToken.RevokedOn = _timeProvider.GetUtcNow().UtcDateTime;
            _dbContext.Update(refreshToken);

            int updateAffectedEntitiesCount = await _dbContext.SaveChangesAsync();
            if(updateAffectedEntitiesCount < 1) { return false; }


            return true;

        }

        public async Task AddRefreshTokenToCookies(string refreshToken, DateTime refreshTokenExpiration, HttpContext httpContextAccessor)
        {
            var refreshTokenCookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                Expires = refreshTokenExpiration,
                SameSite = SameSiteMode.Strict,
                //Path = "/auth/refresh"
            };
            httpContextAccessor.Response.Cookies
                .Append(GateWayHeaders.RefreshTokenHeader,
                refreshToken
                , refreshTokenCookieOptions);


        }
    }
}
