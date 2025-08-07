

namespace Basket.Basket.Data
{
    public class BasketDBContext : DbContext
    {
        public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
        public DbSet<ShoppingCartItem> ShoppingCartItems => Set<ShoppingCartItem>();
        public DbSet<OutBoxMessage> OutBoxMessages => Set<OutBoxMessage>();
        public BasketDBContext(DbContextOptions<BasketDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("basket");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);

        }

    }
}
