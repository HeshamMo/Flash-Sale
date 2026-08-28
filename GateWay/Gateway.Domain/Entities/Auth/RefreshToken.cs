using System.ComponentModel.DataAnnotations.Schema;

namespace Gateway.Domain.Entities.Auth
{
    public class RefreshToken
    {

        public Guid UserId { get; set; }
        public int Id { get; set; }
        public required string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedoOn { get; set; }
        public DateTime? RevokedOn { get; set; }


        [NotMapped]
        public bool isValid => DateTime.UtcNow <= ExpiresOn && RevokedOn is null;
        [NotMapped]
        public bool isRevoked => RevokedOn is not null;

        public virtual User User { get; set; }
    }
}
