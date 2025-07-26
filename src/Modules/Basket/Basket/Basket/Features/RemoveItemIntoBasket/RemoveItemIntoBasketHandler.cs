namespace Basket.Basket.Features.RemoveItemIntoBasket
{
    public record RemoveItemIntoBasketCommand(string UserName, Guid ProductId) : ICommand<RemoveItemIntoBasketResult>;
    public record RemoveItemIntoBasketResult(Guid Id);
    public class CreateBasketCommandValidator : AbstractValidator<RemoveItemIntoBasketCommand>
    {
        public CreateBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is Required");
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product is Required");

        }
    }
    public class RemoveItemIntoBasketHandler(BasketDBContext dBContext) : ICommandHandler<RemoveItemIntoBasketCommand, RemoveItemIntoBasketResult>
    {
        public async Task<RemoveItemIntoBasketResult> Handle(RemoveItemIntoBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await dBContext.ShoppingCarts.Include(x => x.Items).SingleOrDefaultAsync(x => x.UserName == command.UserName);
            if (basket == null)
            {
                throw new BasketNotFoundException(command.UserName);
            }
            basket.RemoveItem(command.ProductId);
            await dBContext.SaveChangesAsync(cancellationToken);

            return new RemoveItemIntoBasketResult(basket.Id);
        }
    }
}
