
using MassTransit;
using Shared.Messaging.Events;

namespace Catalog.EventHandlers
{
    public class ProductPriceChangedEventHandler(ILogger<ProductPriceChangedEventHandler> logger, IBus bus) : INotificationHandler<ProductPriceChangedEvent>
    {
        public async Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event Handled :{DomainEvent}", notification.GetType().Name);


            var integrationEvent = new ProductPriceChangedIntegrationEvent
            {
                ProductId = notification.Product.Id,
                Name = notification.Product.Name,
                Description = notification.Product.Description,
                Category = notification.Product.Category,
                ImageFile = notification.Product.ImageFile,
                Price = notification.Product.Price,

            };
            await bus.Publish(integrationEvent, cancellationToken);
        }
    }
}
