namespace Battleship.Application.Responses;

/// <summary>
/// Represents the response returned after creating a new game.
/// </summary>
/// <param name="Id">
/// The unique identifier of the newly created game.
/// </param>
public record GamePostResponse(
    Guid Id
);