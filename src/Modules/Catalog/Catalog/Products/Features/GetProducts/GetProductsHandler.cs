

namespace Catalog.Products.Features.GetProducts
{
    public record GetProductsQuery(PaginationRequest Request)
    : IQuery<GetProductsResult>;
    public record GetProductsResult(PaginatedResult<ProductDto> Products);
    public class GetProductsHandler(CatalogDBContext dBContext) : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.Request.PageIndex;
            var pageSize = query.Request.PageSize;

            var totalCount = await dBContext.Products.LongCountAsync(cancellationToken);

            var products = await dBContext.Products
                            .AsNoTracking()
                            .OrderBy(p => p.Name)
                            .Skip(pageSize * pageIndex)
                            .Take(pageSize)
                            .ToListAsync(cancellationToken);

            var productDtos = products.Adapt<List<ProductDto>>();

            return new GetProductsResult(new PaginatedResult<ProductDto>(pageIndex, pageSize, totalCount, productDtos));
        }

    }
}
