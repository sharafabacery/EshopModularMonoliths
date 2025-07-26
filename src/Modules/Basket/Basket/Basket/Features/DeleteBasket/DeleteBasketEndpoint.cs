
namespace Basket.Basket.Features.DeleteBasket
{
    public class DeleteBasketEndpoint : ICarterModule
    {
        public record DeleteBasketResponse(bool InSuccess);

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/Basket/{username}", async (string username, ISender sender) =>
            {

                var result = await sender.Send(new DeleteBasketCommand(username));

                var response = result.Adapt<DeleteBasketResponse>();

                return Results.Ok(response);

            })
             .WithName("DeleteBasket")
             .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .WithSummary("DeleteBasket")
             .WithDescription("DeleteBasket");
        }
    }
}
