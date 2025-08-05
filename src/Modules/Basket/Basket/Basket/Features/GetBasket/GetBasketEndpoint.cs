
namespace Basket.Basket.Features.GetBasket
{
    public record GetBasketdResponse(ShoppingCartDto ShoppingCart);
    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{username}", async (string username, ISender sender) =>
            {

                var result = await sender.Send(new GetBasketQuery(username));

                var response = result.Adapt<GetBasketdResponse>();

                return Results.Ok(response);

            })
            .WithName("GetBasketd")
            .Produces<GetBasketdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("GetBasketd")
            .WithDescription("GetBasketd")
            .RequireAuthorization();
        }
    }
}
