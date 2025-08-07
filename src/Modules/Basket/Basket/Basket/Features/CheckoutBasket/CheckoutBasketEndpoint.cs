﻿namespace Basket.Basket.Features.CheckoutBasket
{
    public record CheckoutBasketRequest(BasketCheckoutDto Basket);
    public record CheckoutBasketResponse(bool IsSuccess);
    public class CheckoutBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/checkout", async (CheckoutBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<CheckoutBasketCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CheckoutBasketResponse>();
                return Results.Ok(response);
            }).WithName("CheckoutBasket")
             .Produces<CheckoutBasketResponse>(StatusCodes.Status201Created)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("CheckoutBasket")
             .WithDescription("CheckoutBasket")
             .RequireAuthorization();
        }
    }
}
