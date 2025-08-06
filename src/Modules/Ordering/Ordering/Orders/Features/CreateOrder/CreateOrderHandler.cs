
using Ordering.Orders.Data;

namespace Ordering.Orders.Features.CreateOrder
{
    public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;
    public record CreateOrderResult(Guid Id);
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("OrderName is Required");
        }
    }
    public class CreateOrderHandler(OrderingDBContext dBContext) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        public async Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = CreateNewOrder(request.Order);
            dBContext.Orders.Add(order);
            await dBContext.SaveChangesAsync(cancellationToken);
            return new CreateOrderResult(order.Id);
        }

        private Order CreateNewOrder(OrderDto order)
        {
            var shippingAddress = Address.Of(order.ShippingAddress.FirstName, order.ShippingAddress.LastName, order.ShippingAddress.EmailAddress, order.ShippingAddress.AddressLine, order.ShippingAddress.Country, order.ShippingAddress.State, order.ShippingAddress.ZipCode);
            var billingAddress = Address.Of(order.BillingAddress.FirstName, order.BillingAddress.LastName, order.BillingAddress.EmailAddress, order.BillingAddress.AddressLine, order.BillingAddress.Country, order.BillingAddress.State, order.BillingAddress.ZipCode);

            var orderModel = Order.Create(id: Guid.NewGuid(), customerId: order.CustomerId, orderName: $"{order.OrderName}_{new Random().Next()}", shippingAddress, billingAddress, payment: Payment.Of(order.Payment.CardNumber, order.Payment.CardName, order.Payment.Expiration, order.Payment.Cvv, order.Payment.PaymentMethod));

            foreach (var item in order.Items)
            {
                orderModel.AddItem(item.OrderId, item.Quantity, item.Price);
            }
            return orderModel;
        }
    }
}
