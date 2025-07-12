
namespace Catalog.Products.Features.UpdateProduct
{
    public record UpdateProductCommand(ProductDto Product)
        : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductHandler(CatalogDBContext dBContext) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product =await dBContext.Products.FindAsync([command.Product.Id],cancellationToken);
            if (product is null) {
                throw new Exception($"Product not found: {command.Product.Id}");
            }
            UpdateProductWithNewValues(product,command.Product);

            dBContext.Products.Update(product);

            await dBContext.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }

        private void UpdateProductWithNewValues(Product product, ProductDto productDto)
        {
            product.Update(productDto.Name,productDto.Category,productDto.Description,productDto.ImageFile,productDto.Price);
        }
    }
}
