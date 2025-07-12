
namespace Catalog.Products.Features.DeleteProduct
{
    public record DeleteProductCommand(Guid id)
    : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);
    public class DeleteProductHandler(CatalogDBContext dBContext) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await dBContext.Products.FindAsync([command.id], cancellationToken);
            if (product is null)
            {
                throw new Exception($"Product not found: {command.id}");
            }
            dBContext.Products.Remove(product);
            await dBContext.SaveChangesAsync();
            return new DeleteProductResult(true);

        }
    }
}
