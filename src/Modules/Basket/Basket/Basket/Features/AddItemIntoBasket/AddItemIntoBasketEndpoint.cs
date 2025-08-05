namespace Basket.Basket.Features.AddItemIntoBasket
{
    public record AddItemIntoBasketRequest(string UserName, ShoppingCartItemDto ShoppingCartItem);
    public record AddItemIntoBasketResponse(Guid Id);


    public class AddItemIntoBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/{username}/items", async ([FromRoute] string username, [FromBody] AddItemIntoBasketRequest request, ISender sender) =>
            {
                var result = await sender.Send(new AddItemIntoBasketCommand(username, request.ShoppingCartItem));

                var response = result.Adapt<AddItemIntoBasketResponse>();

                return Results.Created($"/Basket/{response.Id}", response);

            })
                .WithName("AddItemIntoBasket")
                .Produces<AddItemIntoBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("AddItemIntoBasket")
                .WithDescription("AddItemIntoBasket")
                .RequireAuthorization();
        }
    }
}
