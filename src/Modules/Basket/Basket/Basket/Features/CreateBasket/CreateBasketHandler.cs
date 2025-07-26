namespace Basket.Basket.Features.CreateBasket
{
    public record CreateBasketCommand(ShoppingCartDto ShoppingCart) : ICommand<CreateBasketResult>;

    public record CreateBasketResult(Guid Id);
    public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
    {
        public CreateBasketCommandValidator()
        {
            RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("UserName is Required");

        }
    }
    public class CreateBasketHandler(BasketDBContext dBContext) : ICommandHandler<CreateBasketCommand, CreateBasketResult>
    {
        public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
        {
            var shoppingCart = CreateNewBasket(command.ShoppingCart);

            dBContext.ShoppingCarts.Add(shoppingCart);

            await dBContext.SaveChangesAsync(cancellationToken);

            return new CreateBasketResult(shoppingCart.Id);
        }

        private ShoppingCart CreateNewBasket(ShoppingCartDto shoppingCartDto)
        {

            var newBasket = ShoppingCart.Create(Guid.NewGuid(), shoppingCartDto.UserName);
            shoppingCartDto.Items.ForEach(item =>
            {
                newBasket.AddItem(item.ProductId, item.Quantity, item.Color, item.Price, item.ProductName);

            });

            return newBasket;
        }
    }
}
