﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog
{
    public static class CatalogModule
    {
        public static IServiceCollection AddCatalogModule(this IServiceCollection services,IConfiguration configuration) {

            var connectionString = configuration.GetConnectionString("Database");

            services.AddDbContext<CatalogDBContext>((options) =>
            {
                options.UseNpgsql(connectionString);
            });

            return services;
        }
        public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
        {
            InitialiseDatabaseAsync(app).GetAwaiter().GetResult();

            return app;
        }

        public static async Task InitialiseDatabaseAsync(IApplicationBuilder app)
        {
            using var scope=app.ApplicationServices.CreateScope();

            var context=scope.ServiceProvider.GetService<CatalogDBContext>();

            await context.Database.MigrateAsync();

        }
    }
}
