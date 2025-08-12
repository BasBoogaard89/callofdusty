namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void InstallServicesFromAssembly(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
    {
        var installers = assembly.GetTypes()
            .Where(type => typeof(IInstaller).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IInstaller>();

        foreach (var installer in installers)
        {
            installer.InstallServices(services, configuration);
        }
    }

    public static void AddServicesByConvention(this IServiceCollection services, Assembly assembly)
    {
        var implementations = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType)
            .ToList();

        foreach (var implementation in implementations)
        {
            var serviceInterface = implementation
                .GetInterfaces()
                .FirstOrDefault(i =>
                    i.Name.Equals($"I{implementation.Name}", StringComparison.Ordinal));

            if (serviceInterface != null)
            {
                services.AddScoped(serviceInterface, implementation);
            }
        }

        AddGenericBaseRepositories(services, assembly);
    }

    public static void AddGenericBaseRepositories(this IServiceCollection services, Assembly assembly)
    {
        var types = assembly.GetTypes();

        var baseRepoDefinition = typeof(IBaseRepository<>);

        var candidates = types
            .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType)
            .ToList();

        foreach (var implementation in candidates)
        {
            var interfaces = implementation.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == baseRepoDefinition);

            foreach (var iface in interfaces)
            {
                services.AddScoped(iface, implementation);
            }
        }
    }


}
