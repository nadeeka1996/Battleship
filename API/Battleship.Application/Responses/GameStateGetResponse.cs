using Battleship.Application.Helpers;
using Battleship.Domain.Enums;
using Battleship.Domain.Models;

namespace Battleship.Application.Responses;

/// <summary>
/// Represents the response containing the current state of a Battleship game.
/// </summary>
public record GameStateGetResponse(
    Guid Id,
    IReadOnlyList<ShotResponse> Shots,
    IReadOnlyList<ShipResponse> Ships,
    bool IsOver
)
{
    /// <summary>
    /// Maps a <see cref="Game"/> domain model to a <see cref="GameStateGetResponse"/> response.
    /// </summary>
    /// <param name="game">The game instance to map.</param>
    /// <returns>A <see cref="GameStateGetResponse"/> representing the current state of the game.</returns>
    public static GameStateGetResponse Map(Game game) => new(
        game.Id,
        game.Shots.Select(s => new ShotResponse(s.Column, s.Row)).ToList(),
        game.Ships.Select(s =>
            new ShipResponse(
                s.Type,
                s.Positions
                    .Where(p => game.Shots.Contains(p, new CoordinateComparer()))
                    .Select(p => new PositionResponse(p.Column, p.Row))
                    .ToList(),
                s.IsSunk
            )
        ).ToList(),
        game.IsOver
    );
}

/// <summary>
/// Represents a shot fired in the game, identified by its column and row.
/// </summary>
/// <param name="Column">The column of the shot.</param>
/// <param name="Row">The row of the shot.</param>
public record ShotResponse(
    char Column,
    int Row
);

/// <summary>
/// Represents a ship in the game, including its type, hit positions, and sunk status.
/// </summary>
/// <param name="Type">The type of the ship.</param>
/// <param name="Positions">The positions of the ship that have been hit.</param>
/// <param name="IsSunk">Indicates whether the ship is sunk.</param>
public record ShipResponse(
    ShipType Type,
    IReadOnlyList<PositionResponse> Positions,
    bool IsSunk
);

/// <summary>
/// Represents a position on the game board, identified by its column and row.
/// </summary>
/// <param name="Column">The column of the position.</param>
/// <param name="Row">The row of the position.</param>
public record PositionResponse(
    char Column,
    int Row
);