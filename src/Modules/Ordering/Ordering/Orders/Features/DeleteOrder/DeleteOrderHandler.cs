

namespace Ordering.Orders.Features.DeleteOrder
{
    public record DeleteOrderCommand(Guid OrderId) : ICommand<DeleteOrderResult>;
    public record DeleteOrderResult(bool IsSuccess);
    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("OrderId is Required");
        }
    }
    public class DeleteOrderHandler(OrderingDBContext dBContext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await dBContext.Orders.FindAsync([command.OrderId], cancellationToken);
            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }
            dBContext.Orders.Remove(order);
            await dBContext.SaveChangesAsync();
            return new DeleteOrderResult(true);
        }
    }
}
