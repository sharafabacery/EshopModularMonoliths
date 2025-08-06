
namespace Ordering.Orders.Features.GetOrders
{
    public record GetOrdersQuery(PaginationRequest Request)
    : IQuery<GetOrdersResult>;
    public record GetOrdersResult(PaginatedResult<OrderDto> Orders);

    public class GetOrdersHandler(OrderingDBContext dBContext) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
    {
        public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.Request.PageIndex;
            var pageSize = query.Request.PageSize;

            var totalCount = await dBContext.Orders.LongCountAsync(cancellationToken);

            var orders = await dBContext.Orders
                            .AsNoTracking()
                            .OrderBy(p => p.OrderName)
                            .Skip(pageSize * pageIndex)
                            .Take(pageSize)
                            .ToListAsync(cancellationToken);

            var orderDtos = orders.Adapt<List<OrderDto>>();

            return new GetOrdersResult(new PaginatedResult<OrderDto>(pageIndex, pageSize, totalCount, orderDtos));
        }

    }
}
