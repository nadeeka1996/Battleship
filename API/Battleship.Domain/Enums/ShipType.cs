namespace Battleship.Domain.Enums;

/// <summary>
/// Represents the different types of ships available in the Battleship game.
/// </summary>
public enum ShipType
{
    /// <summary>
    /// No ship.
    /// </summary>
    None = 0,

    /// <summary>
    /// A battleship, typically the largest ship type.
    /// </summary>
    Battleship = 5,

    /// <summary>
    /// A destroyer, typically a medium-sized ship type.
    /// </summary>
    Destroyer = 4
}