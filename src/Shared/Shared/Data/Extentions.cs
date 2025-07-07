﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Seed;

namespace Shared.Data
{
    public static class Extentions
    {
        public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder applicationBuilder)
            where TContext : DbContext
        {
            MigrateDatabaseAsync<TContext>(applicationBuilder.ApplicationServices).GetAwaiter().GetResult();
            SeedDataAsync(applicationBuilder.ApplicationServices).GetAwaiter().GetResult();
            return applicationBuilder;
        }

        private async static Task MigrateDatabaseAsync<TContext>(IServiceProvider applicationServices) where TContext : DbContext
        {
            using var scope = applicationServices.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TContext>();

            await context.Database.MigrateAsync();
        }

        private static async Task SeedDataAsync(IServiceProvider applicationServices)
        {
            using var scope = applicationServices.CreateScope();

            var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();

            foreach (var seeder in seeders)
            {
                await seeder.SeedAllAsync();
            }

            throw new NotImplementedException();
        }

    }
}
