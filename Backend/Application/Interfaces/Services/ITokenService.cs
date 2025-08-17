namespace Application.Interfaces.Services;

public interface IJwtTokenService
{
    Task<string> CreateToken(IdentityUser user, IList<string> roles);
}
