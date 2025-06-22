using Battleship.Domain.Enums;

namespace Battleship.Domain.Models;

/// <summary>
/// Represents a ship in the Battleship game, including its type, positions, and hit status.
/// </summary>
public record Ship
{
    /// <summary>
    /// Gets the unique identifier for the ship.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Gets the type of the ship (e.g., Battleship, Destroyer).
    /// </summary>
    public ShipType Type { get; init; }

    /// <summary>
    /// Gets the collection of coordinates occupied by the ship on the game board.
    /// </summary>
    public ICollection<Coordinate> Positions { get; init; } = [];

    /// <summary>
    /// Gets a value indicating whether the ship is sunk (all positions have been hit).
    /// </summary>
    public bool IsSunk => Positions.All(p => Hits.Contains(p));

    /// <summary>
    /// Gets the set of coordinates where the ship has been hit.
    /// </summary>
    public HashSet<Coordinate?> Hits { get; } = [];

    private Ship() { }

    /// <summary>
    /// Creates a new ship with the specified type and positions.
    /// </summary>
    /// <param name="type">The type of the ship.</param>
    /// <param name="positions">The collection of coordinates the ship occupies.</param>
    /// <returns>A new <see cref="Ship"/> instance.</returns>
    public static Ship Create(ShipType type, ICollection<Coordinate> positions) => new()
    {
        Type = type,
        Positions = positions
    };

    /// <summary>
    /// Attempts to register a hit on the ship at the specified coordinate.
    /// </summary>
    /// <param name="coordinate">The coordinate to hit.</param>
    /// <returns><c>true</c> if the hit was successful and not previously registered; otherwise, <c>false</c>.</returns>
    public bool TryHit(Coordinate coordinate) =>
        Positions.Contains(coordinate) && Hits.Add(coordinate);
}