using Shared.Data.Seed;

namespace Catalog.Data.Seed
{
    public class CatalogDataSeeder(CatalogDBContext catalogContext) : IDataSeeder
    {
        public async Task SeedAllAsync()
        {
            if (!await catalogContext.Products.AnyAsync())
            {
                await catalogContext.Products.AddRangeAsync(InitialData.Products);
                await catalogContext.SaveChangesAsync();
            }
        }
    }
}
