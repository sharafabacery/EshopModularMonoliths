namespace Catalog.Products.Features.CreateProduct
{
    public record CreateProductCommand(ProductDto Product)
        : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Product.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.Product.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price is required");

        }
    }
    //represent bussiness logic ,sqrs
    internal class CreateProductHandler(CatalogDBContext dBContext, ILogger<CreateProductHandler> logger) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {

            logger.LogInformation("CreateProductHandler.Handle called with {@coomand}", command);

            var product = CreateNewProduct(command.Product);

            dBContext.Products.Add(product);

            await dBContext.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
        }

        private Product CreateNewProduct(ProductDto productDto)
        {
            var product = Product.Create(Guid.NewGuid(), productDto.Name, productDto.Category, productDto.Description, productDto.ImageFile, productDto.Price);
            return product;
        }
    }
}
