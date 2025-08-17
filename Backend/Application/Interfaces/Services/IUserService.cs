namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<string> Register(RegisterDto dto);
    Task<string> Login(LoginDto dto);
}
