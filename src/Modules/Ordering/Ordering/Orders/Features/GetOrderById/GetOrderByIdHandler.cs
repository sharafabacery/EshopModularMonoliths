namespace Ordering.Orders.Features.GetOrderById
{
    public record GetOrderByIdQuery(Guid OrderId)
: IQuery<GetOrderByIdResult>;
    public record GetOrderByIdResult(OrderDto Product);
    public class GetOrderByIdHandler(OrderingDBContext dBContext) : IQueryHandler<GetOrderByIdQuery, GetOrderByIdResult>
    {
        public async Task<GetOrderByIdResult> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
        {
            var order = await dBContext.Orders.AsNoTracking().SingleOrDefaultAsync(p => p.Id == query.OrderId, cancellationToken);
            if (order is null)
            {
                throw new OrderNotFoundException(query.OrderId);
            }

            var orderDto = order.Adapt<OrderDto>();

            return new GetOrderByIdResult(orderDto);
        }
    }
}
