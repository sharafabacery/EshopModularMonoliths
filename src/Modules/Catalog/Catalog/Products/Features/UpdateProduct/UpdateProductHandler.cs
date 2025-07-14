using Catalog.Products.Exceptions;

namespace Catalog.Products.Features.UpdateProduct
{
    public record UpdateProductCommand(ProductDto Product)
        : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Product.Id).NotEmpty().WithMessage("Product Id is Required");
            RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price is required");

        }
    }
    public class UpdateProductHandler(CatalogDBContext dBContext) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await dBContext.Products.FindAsync([command.Product.Id], cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException(command.Product.Id);
            }
            UpdateProductWithNewValues(product, command.Product);

            dBContext.Products.Update(product);

            await dBContext.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }

        private void UpdateProductWithNewValues(Product product, ProductDto productDto)
        {
            product.Update(productDto.Name, productDto.Category, productDto.Description, productDto.ImageFile, productDto.Price);
        }
    }
}
