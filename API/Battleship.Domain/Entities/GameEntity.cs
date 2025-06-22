namespace Battleship.Domain.Entities;

/// <summary>
/// Represents a persisted Battleship game entity, including its unique identifier and serialized state.
/// </summary>
public class GameEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the game entity.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the serialized representation of the game state.
    /// </summary>
    public string SerializedGameState { get; set; } = string.Empty;
}