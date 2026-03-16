using AlAshmar.Domain.Entities.Abstraction;

namespace AlAshmar.Domain.Entities.Users;

public class RefreshToken : Entity<Guid>
{
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public Guid UserId { get; set; }
    public bool IsRevoked { get; set; }

    public User User { get; set; } = null!;
}
