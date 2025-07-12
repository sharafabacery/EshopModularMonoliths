using Catalog.Products.Features.GetProductByCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Catalog.Products.Features.GetProductById
{
    public record GetProductByIdQuery(Guid Id)
: IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(ProductDto Product);
    public class GetProductById(CatalogDBContext dBContext) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await dBContext.Products.AsNoTracking().SingleOrDefaultAsync(p=>p.Id==query.Id, cancellationToken);
            if (product is null)
            {
                throw new Exception($"Product not found: {query.Id}");
            }

            var productDto = product.Adapt<ProductDto>();

            return new GetProductByIdResult(productDto);
        }
    }

}
