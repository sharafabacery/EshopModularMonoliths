﻿

namespace Ordering.Orders.EventHandlers
{
    public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger) : INotificationHandler<OrderCreatedEvent>
    {
        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event Handled :{DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
