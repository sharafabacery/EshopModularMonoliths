namespace Catalog.Data
{
    public class CatalogDBContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();
        public CatalogDBContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("catalog");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
            
        }

    }
}
