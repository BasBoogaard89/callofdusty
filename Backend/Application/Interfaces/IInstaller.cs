namespace Application.Interfaces;

public interface IInstaller
{
    void InstallServices(IServiceCollection services, IConfiguration configuration);
}
