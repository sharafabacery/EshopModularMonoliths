

namespace Ordering.Orders.Models
{
    public class Order : Aggregate<Guid>
    {
        private readonly List<OrderItem> _items = new();
        public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
        public Guid CustomerId { get; private set; } = default!;
        public string OrderName { get; private set; } = default!;
        public Address ShippingAddress { get; private set; } = default!;
        public Address BillingAddress { get; private set; } = default!;
        public Payment Payment { get; private set; } = default!;
        public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

        public static Order Create(Guid id, Guid customerId, string orderName, Address shippingAddress, Address billingAddress, Payment payment)
        {
            var order = new Order() { Id = id, CustomerId = customerId, ShippingAddress = shippingAddress, BillingAddress = billingAddress, Payment = payment };
            order.AddDomainEvent(new OrderCreatedEvent(order));
            return order;
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
                var newItem = new OrderItem(Id, productId, quantity, price);
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
