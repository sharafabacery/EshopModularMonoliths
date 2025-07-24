
namespace Basket.Basket.Models
{
    public class ShoppingCartItem : Entity<Guid>
    {
        public Guid ShoppingCartId { get; private set; } = default!;
        public Guid ProductId { get; private set; } = default!;
        public int Quantity { get; internal set; } = default!;
        public string Color { get; private set; } = default!;
        public decimal Price { get; private set; } = default!;
        public string ProductName { get; private set; } = default!;

        internal ShoppingCartItem(Guid ShoppingCartId, Guid ProductId, int Quantity, string Color, decimal Price, string ProductName)
        {
            this.ShoppingCartId = ShoppingCartId;
            this.ProductId = ProductId;
            this.Quantity = Quantity;
            this.Color = Color;
            this.Price = Price;
            this.ProductName = ProductName;

        }
    }
}
