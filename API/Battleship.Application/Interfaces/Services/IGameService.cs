using Battleship.Application.Common;
using Battleship.Application.Requests;
using Battleship.Application.Responses;

namespace Battleship.Application.Interfaces.Services;

/// <summary>
/// Provides methods for managing Battleship games, including creation, shooting, and retrieving game state.
/// </summary>
public interface IGameService
{
    /// <summary>
    /// Creates a new Battleship game instance.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, containing a <see cref="Result{T}"/> with the created game's details.
    /// </returns>
    Task<Result<GamePostResponse>> CreateAsync();

    /// <summary>
    /// Registers a shot in the specified game.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game.</param>
    /// <param name="request">The shot request containing the target coordinates.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, containing a <see cref="Result{T}"/> with the result of the shot.
    /// </returns>
    Task<Result<GameShotPostResponse>> PostShotAsync(Guid gameId, GameShotPostRequest request);

    /// <summary>
    /// Retrieves the current state of the specified game.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, containing a <see cref="Result{T}"/> with the current game state.
    /// </returns>
    Task<Result<GameStateGetResponse>> GetStateAsync(Guid gameId);
}
