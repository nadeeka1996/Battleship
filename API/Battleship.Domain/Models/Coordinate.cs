namespace Battleship.Domain.Models;

/// <summary>
/// Represents a coordinate on the Battleship game board, defined by a column and a row.
/// </summary>
public record Coordinate
{
    /// <summary>
    /// Gets the column of the coordinate (e.g., 'A', 'B', etc.).
    /// </summary>
    public char Column { get; init; }

    /// <summary>
    /// Gets the row of the coordinate (e.g., 1, 2, etc.).
    /// </summary>
    public int Row { get; init; }

    private Coordinate() { }

    /// <summary>
    /// Creates an empty coordinate with default values.
    /// </summary>
    /// <returns>A new <see cref="Coordinate"/> instance with default values.</returns>
    public static Coordinate Empty() => new();

    /// <summary>
    /// Creates a new coordinate with the specified column and row.
    /// </summary>
    /// <param name="column">The column character of the coordinate.</param>
    /// <param name="row">The row number of the coordinate.</param>
    /// <returns>A new <see cref="Coordinate"/> instance with the specified column and row.</returns>
    public static Coordinate Create(char column, int row) => new()
    {
        Column = column,
        Row = row
    };
}