
namespace Basket.Basket.Repository
{
    public class BasketRepository(BasketDBContext dBContext) : IBasketRepository
    {
        public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {

            dBContext.ShoppingCarts.Add(basket);
            await dBContext.SaveChangesAsync(cancellationToken);
            return basket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await GetBasket(userName, false, cancellationToken);
            dBContext.Remove(basket);
            await dBContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<ShoppingCart> GetBasket(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            var query = dBContext.ShoppingCarts.Include(e => e.Items).Where(e => e.UserName == userName);
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            var basket = await query.SingleOrDefaultAsync(cancellationToken);
            return basket ?? throw new BasketNotFoundException(userName);
        }

        public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
        {
            return await dBContext.SaveChangesAsync(cancellationToken);
        }
    }
}
