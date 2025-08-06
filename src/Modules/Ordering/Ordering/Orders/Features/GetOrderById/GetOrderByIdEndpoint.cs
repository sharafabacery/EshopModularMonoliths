﻿namespace Ordering.Orders.Features.GetOrderById
{
    public record GetOrderByIdResponse(OrderDto Product);

    public class GetOrderByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/{id}", async (Guid id, ISender sender) =>
            {

                var result = await sender.Send(new GetOrderByIdQuery(id));

                var response = result.Adapt<GetOrderByIdResponse>();

                return Results.Ok(response);

            })
            .WithName("GetOrderById")
            .Produces<GetOrderByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("GetOrderById")
            .WithDescription("GetOrderById");
        }
    }
}
