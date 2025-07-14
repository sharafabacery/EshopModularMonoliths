using Catalog.Products.Exceptions;

namespace Catalog.Products.Features.DeleteProduct
{
    public record DeleteProductCommand(Guid ProductId)
    : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product Id is Required");
        }
    }
    public class DeleteProductHandler(CatalogDBContext dBContext) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await dBContext.Products.FindAsync([command.ProductId], cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException(command.ProductId);
            }
            dBContext.Products.Remove(product);
            await dBContext.SaveChangesAsync();
            return new DeleteProductResult(true);

        }
    }
}
