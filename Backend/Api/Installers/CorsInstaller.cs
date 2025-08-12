namespace Api.Installers;

public class CorsInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
            options.AddPolicy("AllowAll", p =>
            p.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));
    }
}
