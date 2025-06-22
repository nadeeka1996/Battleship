using Battleship.Application.Common;
using Battleship.Application.Interfaces.Services;
using Battleship.Application.Requests;

namespace Battleship.Api.Endpoints;

public static class GameEndpoints
{
    public static void MapGameEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/games")
            .WithTags(nameof(GameEndpoints));

        group.MapPost("/", Create);

        group.MapGet("/{id:guid}", Get);

        group.MapPost("/{id:guid}/shots", PostShot);
    }

    public static async Task<IResult> Create(
        IGameService service
    )
    {
        var result = await service.CreateAsync();
        return !result 
            ? Results.BadRequest(Envelope.Error(result.Error ?? nameof(Results.BadRequest)))
            : Results.Created($"/games/{result.Value!.Id}", result.Value);
    }

    public static async Task<IResult> Get(
        Guid id,
        IGameService service
    )
    {
        var result = await service.GetStateAsync(id);
        return !result 
            ? Results.NotFound(Envelope.Error(result.Error ?? nameof(Results.NotFound)))
            : Results.Ok(result.Value);
    }

    public static async Task<IResult> PostShot(
        Guid id,
        GameShotPostRequest request,
        IGameService service
    )
    {
        var result = await service.PostShotAsync(id, request);

        if (!result)
            return result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) is true 
                ? Results.NotFound(Envelope.Error(result.Error ?? nameof(Results.NotFound)))
                : Results.BadRequest(Envelope.Error(result.Error ?? nameof(Results.BadRequest)));

        return Results.Ok(result.Value);
    }
}
