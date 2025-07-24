namespace Basket.Basket.Models
{
    public class ShoppingCart : Aggregate<Guid>
    {
        public string UserName { get; private set; } = default!;

        public readonly List<ShoppingCartItem> _items = new();
        public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();
        public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

        public static ShoppingCart Create(Guid id, string username)
        {
            ArgumentException.ThrowIfNullOrEmpty(username);
            ShoppingCart cart = new ShoppingCart() { Id = id, UserName = username };
            return cart;
        }
        public void AddItem(Guid productId, int quantity, string color, decimal price, string productName)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var exitingItem = Items.FirstOrDefault(x => x.ProductId == productId);
            if (exitingItem != null)
            {
                exitingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new ShoppingCartItem(Id, productId, quantity, color, price, productName);
                _items.Add(newItem);
            }
        }
        public void RemoveItem(Guid productId)
        {

            var exitingItem = Items.FirstOrDefault(x => x.ProductId == productId);
            if (exitingItem != null)
            {
                _items.Remove(exitingItem);
            }

        }
    }
}
