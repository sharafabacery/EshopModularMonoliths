using Carter;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Extension
{
    public static class CarterExtension
    {
        public static IServiceCollection AddCarterWithAssemblies(this IServiceCollection services, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                services.AddCarter(configurator: config =>
                {
                    var modules = assembly.GetTypes()
                            .Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();

                    config.WithModules(modules);
                });
            }
            return services;
        }
    }
}
