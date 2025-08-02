
namespace Basket.Basket.Features.UpdateItemPriceInBasket
{
    public record UpdateItemPriceInBasketCommand(Guid ProductId, decimal Price) : ICommand<UpdateItemPriceInBasketResult>;
    public record UpdateItemPriceInBasketResult(bool IsSuccess);
    public class UpdateItemPriceInBasketCommandValidator : AbstractValidator<UpdateItemPriceInBasketCommand>
    {
        public UpdateItemPriceInBasketCommandValidator()
        {
            RuleFor(b => b.ProductId).NotEmpty().WithMessage("ProductId is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price Must be greater than 0");
        }
    }
    public class UpdateItemPriceInBasketHandler(BasketDBContext dBContext) : ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceInBasketResult>
    {
        public async Task<UpdateItemPriceInBasketResult> Handle(UpdateItemPriceInBasketCommand request, CancellationToken cancellationToken)
        {
            var itemsToUpdate = await dBContext.ShoppingCartItems.Where(x => x.ProductId == request.ProductId).ToListAsync();

            if (!itemsToUpdate.Any())
            {
                return new UpdateItemPriceInBasketResult(false);
            }
            foreach (var item in itemsToUpdate)
            {
                item.UpdatePrice(request.Price);
            }
            await dBContext.SaveChangesAsync(cancellationToken);
            return new UpdateItemPriceInBasketResult(true);
        }
    }
}
