﻿using Catalog.Contracts.Products.Features.GetProductById;

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
    public class AddItemIntoBasketHandler(IBasketRepository basketRepository, ISender sender) : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
    {
        public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetBasket(command.UserName, false, cancellationToken);

            var product = await sender.Send(new GetProductByIdQuery(command.ShoppingCartItem.ProductId));

            basket.AddItem(product.Product.Id, command.ShoppingCartItem.Quantity, command.ShoppingCartItem.Color, product.Product.Price, product.Product.Name);
            await basketRepository.SaveChangesAsync(command.UserName, cancellationToken);
            return new AddItemIntoBasketResult(basket.Id);

        }
    }
}
