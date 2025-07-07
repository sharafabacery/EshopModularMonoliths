using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.DDD;

namespace Shared.Data.Interceptors
{
    public class DispatchDomainEventInterceptor(IMediator mediator) : SaveChangesInterceptor
    {
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvent(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvent(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }

        private async Task DispatchDomainEvent(DbContext? context)
        {
            if (context == null) return;
            var aggregates = context.ChangeTracker
                            .Entries<IAggregate>()
                            .Where(e => e.Entity.DomainEvents.Any())
                            .Select(e => e.Entity);
            var domainEvents= aggregates
                              .SelectMany(e => e.DomainEvents)
                              .ToList();

            aggregates.ToList().ForEach(e=>e.clearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }

        }
    }
}
