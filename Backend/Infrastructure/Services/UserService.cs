namespace Infrastructure.Services;

public class UserService(UserManager<ApplicationUser> userManager, IJwtTokenService tokenService) : IUserService
{
    public async Task<string> Register(RegisterDto dto)
    {
        var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email };
        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return result.Errors.ToString();

        await userManager.AddToRoleAsync(user, "User");

        var roles = await userManager.GetRolesAsync(user);
        var token = await tokenService.CreateToken(user, roles);

        return "";
    }

    public async Task<string> Login(LoginDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null) return null;

        var ok = await userManager.CheckPasswordAsync(user, dto.Password);
        if (!ok) return null;

        var roles = await userManager.GetRolesAsync(user);
        var token = await tokenService.CreateToken(user, roles);

        return token;
    }
}
