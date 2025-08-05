

namespace Basket.Basket.Features.CreateBasket
{
    public record CreateBasketEndpointRequest(ShoppingCartDto ShoppingCart);
    public record CreateBasketEndpointResponse(Guid Id);
    public class CreateBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (CreateBasketEndpointRequest request, ISender sender, ClaimsPrincipal user) =>
            {
                var username = user.Identity!.Name;
                var updatedShoppingCart = request.ShoppingCart with { UserName = username };
                var command = new CreateBasketCommand(updatedShoppingCart);

                var result = await sender.Send(command);

                var response = result.Adapt<CreateBasketEndpointResponse>();

                return Results.Created($"/Basket/{response.Id}", response);

            }).WithName("CreateBasket")
             .Produces<CreateBasketEndpointResponse>(StatusCodes.Status201Created)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("CreateBasket")
             .WithDescription("CreateBasket")
             .RequireAuthorization();
        }
    }
}
