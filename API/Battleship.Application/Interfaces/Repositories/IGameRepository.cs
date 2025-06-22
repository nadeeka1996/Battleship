using Battleship.Domain.Models;

namespace Battleship.Application.Interfaces.Repositories;

/// <summary>
/// Defines methods for managing <see cref="Game"/> entities in a repository.
/// </summary>
public interface IGameRepository
{
    /// <summary>
    /// Creates a new <see cref="Game"/> in the repository.
    /// </summary>
    /// <param name="game">The game to create.</param>
    /// <returns>The created <see cref="Game"/>.</returns>
    Task<Game> CreateAsync(Game game);

    /// <summary>
    /// Retrieves a <see cref="Game"/> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game.</param>
    /// <returns>The <see cref="Game"/> if found; otherwise, <c>null</c>.</returns>
    Task<Game?> GetAsync(Guid id);

    /// <summary>
    /// Persists changes to an existing <see cref="Game"/> in the repository.
    /// </summary>
    /// <param name="game">The game to save.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveAsync(Game game);
}
