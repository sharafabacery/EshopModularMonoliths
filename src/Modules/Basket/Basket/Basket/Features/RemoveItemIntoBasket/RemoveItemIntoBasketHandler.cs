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
    public class RemoveItemIntoBasketHandler(IBasketRepository basketRepository) : ICommandHandler<RemoveItemIntoBasketCommand, RemoveItemIntoBasketResult>
    {
        public async Task<RemoveItemIntoBasketResult> Handle(RemoveItemIntoBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetBasket(command.UserName, false, cancellationToken);
            basket.RemoveItem(command.ProductId);
            await basketRepository.SaveChangesAsync();

            return new RemoveItemIntoBasketResult(basket.Id);
        }
    }
}
