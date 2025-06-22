using Battleship.Domain.Models;

namespace Battleship.Application.Helpers;

/// <summary>
/// Provides functionality to parse string representations of Battleship board coordinates.
/// </summary>
public static class CoordinateParser
{
    /// <summary>
    /// Attempts to parse a string input into a <see cref="Coordinate"/> object.
    /// </summary>
    /// <param name="input">The string input representing the coordinate (e.g., "A1", "J10").</param>
    /// <param name="coordinate">
    /// When this method returns, contains the parsed <see cref="Coordinate"/> if parsing succeeded, or <c>null</c> if parsing failed.
    /// </param>
    /// <returns>
    /// <c>true</c> if the input was successfully parsed into a <see cref="Coordinate"/>; otherwise, <c>false</c>.
    /// </returns>
    public static bool TryParse(string input, out Coordinate coordinate)
    {
        coordinate = Coordinate.Empty();
        if (string.IsNullOrWhiteSpace(input) || input.Length < 2 || input.Length > 3)
            return false;

        input = input.ToUpperInvariant().Trim();
        char column = input[0];
        if (column is < 'A' or > 'J')
            return false;

        if (!int.TryParse(input[1..], out int row))
            return false;

        if (row is < 1 or > 10)
            return false;

        coordinate = Coordinate.Create(column, row);
        return true;
    }
}