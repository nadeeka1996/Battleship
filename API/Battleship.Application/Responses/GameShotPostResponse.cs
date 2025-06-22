using Battleship.Application.Common.Enums;

namespace Battleship.Application.Responses;

/// <summary>
/// Represents the response after a shot is made in the Battleship game.
/// </summary>
/// <param name="Hit">Indicates whether the shot was a hit.</param>
/// <param name="Sunk">Indicates whether a ship was sunk as a result of the shot.</param>
/// <param name="Victory">Indicates whether the shot resulted in a victory (end of game).</param>
public sealed record GameShotPostResponse(
    bool Hit,
    bool Sunk = false,
    bool Victory = false
)
{
    /// <summary>
    /// Maps a <see cref="ShotResult"/> to a <see cref="GameShotPostResponse"/>.
    /// </summary>
    /// <param name="result">The result of the shot.</param>
    /// <returns>
    /// A <see cref="GameShotPostResponse"/> representing the outcome of the shot.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the <paramref name="result"/> is not a valid <see cref="ShotResult"/>.
    /// </exception>
    public static GameShotPostResponse Map(ShotResult result) => result switch
    {
        ShotResult.Miss => new(false),
        ShotResult.Hit => new(true),
        ShotResult.Sunk => new(true, true),
        ShotResult.EndGame => new(true, true, true),
        _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
    };
}