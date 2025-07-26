namespace Basket.Basket.Features.AddItemIntoBasket
{
    public record AddItemIntoBasketCommand(string UserName, ShoppingCartItemDto ShoppingCartItem) : ICommand<AddItemIntoBasketResult>;
    public record AddItemIntoBasketResult(Guid Id);
    public class AddItemIntoBasketCommandValidator : AbstractValidator<AddItemIntoBasketCommand>
    {
        public AddItemIntoBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is Required");
            RuleFor(x => x.ShoppingCartItem.ProductId).NotEmpty().WithMessage("ProductId is Required");
            RuleFor(x => x.ShoppingCartItem.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");

        }
    }
    public class AddItemIntoBasketHandler(BasketDBContext dBContext) : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
    {
        public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await dBContext.ShoppingCarts.Include(x => x.Items).SingleOrDefaultAsync(x => x.UserName == command.UserName);
            if (basket == null)
            {
                throw new BasketNotFoundException(command.UserName);
            }
            basket.AddItem(command.ShoppingCartItem.ProductId, command.ShoppingCartItem.Quantity, command.ShoppingCartItem.Color, command.ShoppingCartItem.Price, command.ShoppingCartItem.ProductName);
            await dBContext.SaveChangesAsync(cancellationToken);
            return new AddItemIntoBasketResult(basket.Id);

        }
    }
}
