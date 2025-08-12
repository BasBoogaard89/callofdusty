namespace Infrastructure.Installers;

public class DbContextInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CallOfDusty;Trusted_Connection=True;Integrated Security=True;")
        );
    }
}
