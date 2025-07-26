
namespace Basket.Basket.Features.RemoveItemIntoBasket
{
    public record RemoveItemIntoBasketResponse(Guid Id);
    public class RemoveItemIntoBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/Basket/{username}/items/{productId}", async ([FromRoute] string username, [FromRoute] Guid productId, ISender sender) =>
            {

                var result = await sender.Send(new RemoveItemIntoBasketCommand(username, productId));

                var response = result.Adapt<RemoveItemIntoBasketResponse>();

                return Results.Ok(response);

            })
             .WithName("RemoveItemIntoBasket")
             .Produces<RemoveItemIntoBasketResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .WithSummary("RemoveItemIntoBasket")
             .WithDescription("RemoveItemIntoBasket");
        }
    }
}
