using Battleship.Domain.Models;

namespace Battleship.Application.Helpers;

/// <summary>
/// Provides equality comparison logic for <see cref="Coordinate"/> instances, 
/// supporting null-safe comparisons.
/// </summary>
internal sealed class CoordinateComparer : IEqualityComparer<Coordinate?>
{
    /// <summary>
    /// Determines whether two <see cref="Coordinate"/> instances are equal.
    /// </summary>
    /// <param name="x">The first coordinate to compare.</param>
    /// <param name="y">The second coordinate to compare.</param>
    /// <returns>
    /// <c>true</c> if both coordinates are null or both are non-null and have the same column and row values; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(Coordinate? x, Coordinate? y)
        => BothNull(x, y) || BothNonNullAndEqual(x, y);

    /// <summary>
    /// Returns a hash code for the specified <see cref="Coordinate"/> instance.
    /// </summary>
    /// <param name="obj">The coordinate for which to get the hash code.</param>
    /// <returns>
    /// A hash code based on the column and row values of the coordinate, or 0 if the coordinate is <c>null</c>.
    /// </returns>
    public int GetHashCode(Coordinate? obj)
        => obj is null
            ? 0
            : HashCode.Combine(obj.Column, obj.Row);

    private static bool BothNull(Coordinate? x, Coordinate? y)
        => x is null && y is null;

    private static bool BothNonNullAndEqual(Coordinate? x, Coordinate? y)
        => x is not null
            && y is not null
            && x.Column == y.Column
            && x.Row == y.Row;
}
