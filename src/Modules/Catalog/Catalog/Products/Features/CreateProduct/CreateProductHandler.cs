using System.Windows.Input;


namespace Catalog.Products.Features.CreateProduct
{
    public record CreateProductCommand(ProductDto Product)
        : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    //represent bussiness logic ,sqrs
    public class CreateProductHandler(CatalogDBContext dBContext) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {

            var product=CreateNewProduct(command.Product);
            
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
