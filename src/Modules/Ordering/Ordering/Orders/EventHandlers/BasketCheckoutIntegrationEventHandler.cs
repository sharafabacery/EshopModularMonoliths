using MassTransit;
using Ordering.Orders.Features.CreateOrder;
using Shared.Messaging.Events;

namespace Ordering.Orders.EventHandlers
{
    public class BasketCheckoutIntegrationEventHandler(ISender sender, ILogger<BasketCheckoutIntegrationEventHandler> logger) : IConsumer<BasketCheckoutIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<BasketCheckoutIntegrationEvent> context)
        {
            logger.LogInformation("Integration Event Handled: {IntegrationEvent}", context.Message.GetType().Name);
            var CreateOrderCommand = MapToCreateOrderEvent(context.Message);
            await sender.Send(CreateOrderCommand);
        }

        private CreateOrderCommand MapToCreateOrderEvent(BasketCheckoutIntegrationEvent message)
        {
            var addressDto = new AddressDto(message.FirstName, message.LastName, message.EmailAddress, message.AddressLine, message.Country, message.State, message.ZipCode);
            var paymentDto = new PaymentDto(message.CardNumber, message.CardName, message.Expiration, message.Cvv, message.PaymentMethod);
            var orderId = Guid.NewGuid();
            var orderDto = new OrderDto(Id: orderId, CustomerId: message.CustomerId, OrderName: message.UserName, ShippingAddress: addressDto, BillingAddress: addressDto, Payment: paymentDto,
                Items: [
                    new OrderItemDto(orderId,new Guid("afa3b9f9-5092-4588-830e-e308392da8c0"),1,(decimal)89.99)
                    ]);
            return new CreateOrderCommand(orderDto);
        }
    }
}
