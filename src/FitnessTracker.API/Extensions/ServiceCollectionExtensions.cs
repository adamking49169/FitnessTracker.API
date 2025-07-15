using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.Hosting;

namespace FitnessTracker.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServicesForFunctions(this IServiceCollection services)
    {
        // Configure Functions worker
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults() // This method is part of the Microsoft.Azure.Functions.Worker.Extensions namespace
            .ConfigureServices(s =>
            {
                // reuse the same DI container
                foreach (var svc in services)

                    s.Add(svc);
            })
            .Build();
        return services;
    }
}
