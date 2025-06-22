namespace Battleship.Domain.Models;

/// <summary>
/// Represents a Battleship game, containing ships and tracking shots.
/// </summary>
public record Game
{
    /// <summary>
    /// Gets the unique identifier for the game.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Gets the collection of ships in the game.
    /// </summary>
    public ICollection<Ship> Ships { get; init; } = [];

    /// <summary>
    /// Gets or sets the set of coordinates where shots have been fired.
    /// </summary>
    public HashSet<Coordinate> Shots { get; set; } = [];

    private Game() { }

    /// <summary>
    /// Creates a new instance of the <see cref="Game"/> class.
    /// </summary>
    /// <returns>A new <see cref="Game"/> instance.</returns>
    public static Game Create() => new();

    /// <summary>
    /// Gets a value indicating whether the game is over (all ships are sunk).
    /// </summary>
    public bool IsOver => Ships.All(s => s.IsSunk);

    /// <summary>
    /// Adds a ship to the game.
    /// </summary>
    /// <param name="ship">The ship to add.</param>
    public void AddShip(Ship ship) => Ships.Add(ship);

    /// <summary>
    /// Attempts to register a shot at the specified coordinate.
    /// </summary>
    /// <param name="coordinate">The coordinate to shoot at.</param>
    /// <param name="hitShip">
    /// When this method returns, contains the ship that was hit, or <c>null</c> if no ship was hit.
    /// </param>
    /// <returns>
    /// <c>true</c> if the shot was registered (not a duplicate); otherwise, <c>false</c>.
    /// </returns>
    public bool TryRegisterShot(Coordinate coordinate, out Ship? hitShip)
    {
        hitShip = null;
        if (!Shots.Add(coordinate))
            return false;

        hitShip = Ships.FirstOrDefault(ship => ship.Positions.Contains(coordinate));
        hitShip?.TryHit(coordinate);

        return true;
    }
}