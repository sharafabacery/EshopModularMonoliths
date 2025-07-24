namespace Basket.Basket.Features.DeleteBasket
{
    public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool InSuccess);
    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is Required");
        }
    }
    public class DeleteBasketHandler(BasketDBContext dBContext) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await dBContext.ShoppingCarts.SingleOrDefaultAsync(x => x.UserName == command.UserName, cancellationToken);

            if (basket == null)
            {
                throw new BasketNotFoundException(command.UserName);
            }
            dBContext.ShoppingCarts.Remove(basket);
            await dBContext.SaveChangesAsync();
            return new DeleteBasketResult(true);
        }
    }
}
