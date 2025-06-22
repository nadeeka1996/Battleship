using Battleship.Application.Interfaces.Repositories;
using Battleship.Domain.Entities;
using Battleship.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Battleship.Infrastructure.Data.Repositories;

/// <summary>
/// Provides an implementation of <see cref="IGameRepository"/> for managing <see cref="Game"/> entities using Entity Framework Core.
/// </summary>
internal class GameRepository(
    GameDbContext contexts,
    ILogger<GameRepository> logger
) : IGameRepository
{
    private readonly GameDbContext _contexts = contexts;
    private readonly ILogger<GameRepository> _logger = logger;

    /// <inheritdoc />
    public async Task<Game> CreateAsync(Game game)
    {
        _logger.LogInformation("Creating new game with ID: {GameId}", game.Id);

        GameEntity entity = new()
        {
            Id = game.Id,
            SerializedGameState = JsonConvert.SerializeObject(game)
        };

        _contexts.Games.Add(entity);
        await _contexts.SaveChangesAsync();
        _logger.LogInformation("Game with ID: {GameId} created successfully.", game.Id);

        return game;
    }

    /// <inheritdoc />
    public async Task<Game?> GetAsync(Guid id)
    {
        _logger.LogInformation("Retrieving game with ID: {GameId}", id);

        GameEntity? entity = await _contexts.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id);

        if (entity is null)
        {
            _logger.LogWarning("Game with ID: {GameId} not found.", id);
            return null;
        }

        var game = JsonConvert.DeserializeObject<Game>(entity.SerializedGameState);
        _logger.LogInformation("Game with ID: {GameId} retrieved successfully.", id);

        return game;
    }

    /// <inheritdoc />
    public async Task SaveAsync(Game game)
    {
        _logger.LogInformation("Saving game with ID: {GameId}", game.Id);

        GameEntity? entity = await _contexts.Games.FindAsync(game.Id);
        if (entity is null)
        {
            _logger.LogError("Game with ID: {GameId} not found. Cannot save.", game.Id);
            throw new KeyNotFoundException($"Game {game.Id} not found.");
        }

        entity.SerializedGameState = JsonConvert.SerializeObject(game);
        await _contexts.SaveChangesAsync();

        _logger.LogInformation("Game with ID: {GameId} saved successfully.", game.Id);
    }
}