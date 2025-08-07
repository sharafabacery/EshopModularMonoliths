using System.Text.Json;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Basket.Basket.Data.Processors
{
    public class OutboxProcessor(IServiceProvider serviceProvider, IBus bus, ILogger<OutboxProcessor> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<BasketDBContext>();

                    var outboxMessages = await dbContext.OutBoxMessages.Where(m => m.ProcessedOn == null).ToListAsync(stoppingToken);

                    foreach (var message in outboxMessages)
                    {
                        var eventType = Type.GetType(message.Type);
                        if (eventType == null)
                        {
                            logger.LogWarning("Could not Resolve type:{Type}", message.Type);
                            continue;
                        }
                        var eventMessage = JsonSerializer.Deserialize(message.Content, eventType);
                        if (eventMessage == null)
                        {
                            logger.LogWarning("Could not desrialize message:{Message}", message.Content);
                            continue;
                        }
                        await bus.Publish(eventMessage, stoppingToken);
                        message.ProcessedOn = DateTime.UtcNow;
                        logger.LogInformation("Successfully processed outbox message with Id:{Id}", message.Id);

                    }
                    await dbContext.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing outbox messages");

                }
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
