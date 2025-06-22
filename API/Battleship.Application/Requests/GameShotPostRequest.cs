namespace Battleship.Application.Requests;

/// <summary>
/// Represents a request to fire a shot at a specific coordinate in the Battleship game.
/// </summary>
/// <param name="Coordinate">
/// The coordinate (e.g., "C6") where the shot is to be fired.
/// </param>
public record GameShotPostRequest(
    string Coordinate
);