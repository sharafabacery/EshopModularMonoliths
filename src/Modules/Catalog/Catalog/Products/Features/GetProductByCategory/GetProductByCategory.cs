using Catalog.Products.Features.GetProducts;

namespace Catalog.Products.Features.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category)
   : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(IEnumerable<ProductDto> Products);
    public class GetProductByCategory(CatalogDBContext dBContext) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            var products = await dBContext.Products
                           .AsNoTracking()
                           .Where(p => p.Category.Contains(query.Category))
                           .OrderBy(p => p.Name)
                           .ToListAsync(cancellationToken);

            var productDtos = products.Adapt<List<ProductDto>>();

            return new GetProductByCategoryResult(productDtos);
        }
    }
}
