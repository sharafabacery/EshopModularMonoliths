using System.Text.Json;
using Shared.Messaging.Events;

namespace Basket.Basket.Features.CheckoutBasket
{
    public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout) : ICommand<CheckoutBasketResult>;
    public record CheckoutBasketResult(bool IsSuccess);
    public class CheckoutBasketValidator : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketValidator()
        {
            RuleFor(x => x.BasketCheckout).NotNull().WithMessage("BasketCheckout cant be empty");
            RuleFor(x => x.BasketCheckout.UserName).NotEmpty().WithMessage("UserName is Required");

        }
    }
    public class CheckoutBasketHandler(BasketDBContext dBContext) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
    {
        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
        {
            await using var transaction = await dBContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var basket = await dBContext.ShoppingCarts.Include(e => e.Items).Where(e => e.UserName == command.BasketCheckout.UserName).SingleOrDefaultAsync(cancellationToken);
                if (basket == null)
                {
                    throw new BasketNotFoundException(command.BasketCheckout.UserName);
                }
                var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
                eventMessage.TotalPrice = basket.TotalPrice;

                var outboxMessage = new OutBoxMessage { Id = Guid.NewGuid(), Type = typeof(BasketCheckoutIntegrationEvent).AssemblyQualifiedName!, Content = JsonSerializer.Serialize(eventMessage), OccuredOn = DateTime.UtcNow };

                dBContext.OutBoxMessages.Add(outboxMessage);

                dBContext.ShoppingCarts.Remove(basket);
                await dBContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return new CheckoutBasketResult(true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new CheckoutBasketResult(false);
            }
            //var basket = await basketRepository.GetBasket(command.BasketCheckout.UserName, true, cancellationToken);
            //var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
            //eventMessage.TotalPrice = basket.TotalPrice;
            //await bus.Publish(eventMessage, cancellationToken);
            //await basketRepository.DeleteBasket(command.BasketCheckout.UserName, cancellationToken);
            //return new CheckoutBasketResult(true);
        }
    }
}
