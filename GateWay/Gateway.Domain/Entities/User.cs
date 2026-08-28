using Gateway.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;

namespace Gateway.Domain.Entities
{
    public class User:IdentityUser<Guid>
    {

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    }
}
