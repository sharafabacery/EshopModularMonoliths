using Catalog.Data.Seed;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Behaviours;
using Shared.Data.Interceptors;


namespace Catalog
{
    public static class CatalogModule
    {
        public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


            var connectionString = configuration.GetConnectionString("Database");

            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

            services.AddDbContext<CatalogDBContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IDataSeeder, CatalogDataSeeder>();

            return services;
        }
        public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
        {
            app.UseMigration<CatalogDBContext>();
            return app;
        }


    }
}
