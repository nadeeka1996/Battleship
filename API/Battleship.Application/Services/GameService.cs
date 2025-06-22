using Battleship.Application.Common;
using Battleship.Application.Common.Enums;
using Battleship.Application.Helpers;
using Battleship.Application.Interfaces.Repositories;
using Battleship.Application.Interfaces.Services;
using Battleship.Application.Requests;
using Battleship.Application.Responses;
using Battleship.Domain.Enums;
using Battleship.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace Battleship.Application.Services;

/// <summary>
/// Provides methods for managing Battleship games, including creation, shooting, and retrieving game state.
/// </summary>
internal sealed class GameService(
    IGameRepository repository,
    ILogger<GameService> logger
) : IGameService
{
    private readonly IGameRepository _repository = repository;
    private readonly ILogger<GameService> _logger = logger;
    private const int BattleshipCount = 1;
    private const int DestroyerCount = 2;

    /// <inheritdoc/>
    public async Task<Result<GamePostResponse>> CreateAsync()
    {
        _logger.LogInformation("Creating a new game instance.");
        var game = Game.Create();

        IEnumerable<ShipType> shipTypes = [
            .. Enumerable.Repeat(ShipType.Battleship, BattleshipCount),
            .. Enumerable.Repeat(ShipType.Destroyer, DestroyerCount)
        ];

        foreach (ShipType type in shipTypes)
        {
            bool placed = false;
            int attempts = 0;
            while (!placed)
            {
                placed = game.TryPlaceShip(type);
                attempts++;
                if (!placed && attempts % 10 is 0)
                    _logger.LogWarning("Still trying to place {ShipType} after {Attempts} attempts.", type, attempts);
            }
            _logger.LogInformation("{ShipType} placed after {Attempts} attempts.", type, attempts);
        }

        Game created = await _repository.CreateAsync(game);
        _logger.LogInformation("Game created with Id: {GameId}", created.Id);

        return Result<GamePostResponse>.Success(new GamePostResponse(created.Id));
    }

    /// <inheritdoc/>
    public async Task<Result<GameStateGetResponse>> GetStateAsync(Guid gameId)
    {
        _logger.LogInformation("Retrieving state for game {GameId}", gameId);
        var game = await _repository.GetAsync(gameId);
        if (game is null)
        {
            _logger.LogWarning("Game {GameId} not found.", gameId);
            return Result<GameStateGetResponse>.Failure($"Game {gameId} not found.");
        }

        _logger.LogInformation("Game {GameId} state retrieved successfully - {@Game}", gameId, game);
        return Result<GameStateGetResponse>.Success(GameStateGetResponse.Map(game));
    }

    /// <inheritdoc/>
    public async Task<Result<GameShotPostResponse>> PostShotAsync(Guid gameId, GameShotPostRequest request)
    {
        _logger.LogInformation("Processing shot for game {GameId} at coordinate '{Coordinate}'", gameId, request.Coordinate);
        string payload = request.Coordinate.Trim().Trim('"');
        if (string.IsNullOrWhiteSpace(payload))
        {
            _logger.LogWarning("Shot request for game {GameId} missing coordinate.", gameId);
            return Result<GameShotPostResponse>.Failure("Coordinate is required in the request body.");
        }

        if (!CoordinateParser.TryParse(payload, out Coordinate coordinate))
        {
            _logger.LogWarning("Invalid coordinate '{Coordinate}' in shot request for game {GameId}.", payload, gameId);
            return Result<GameShotPostResponse>.Failure("Invalid coordinate. Use A1–J10.");
        }

        Game? game = await _repository.GetAsync(gameId);
        if (game is null)
        {
            _logger.LogWarning("Game {GameId} not found for shot request.", gameId);
            return Result<GameShotPostResponse>.Failure($"Game {gameId} not found.");
        }

        if (game.IsOver)
        {
            _logger.LogInformation("Shot attempted on finished game {GameId}.", gameId);
            return Result<GameShotPostResponse>.Success(GameShotPostResponse.Map(ShotResult.EndGame));
        }

        if (!game.TryRegisterShot(coordinate, out Ship? hitShip))
        {
            _logger.LogWarning("Coordinate {Coordinate} already shot in game {GameId}.", coordinate, gameId);
            return Result<GameShotPostResponse>.Failure("Coordinate already shot.");
        }

        await _repository.SaveAsync(game);

        if (hitShip is null)
        {
            _logger.LogInformation("Shot at {Coordinate} in game {GameId} was a miss.", coordinate, gameId);
            return Result<GameShotPostResponse>.Success(GameShotPostResponse.Map(ShotResult.Miss));
        }

        if (game.IsOver)
        {
            _logger.LogInformation("Shot at {Coordinate} in game {GameId} hit and ended the game.", coordinate, gameId);
            return Result<GameShotPostResponse>.Success(GameShotPostResponse.Map(ShotResult.EndGame));
        }

        if (hitShip.IsSunk)
        {
            _logger.LogInformation("Shot at {Coordinate} in game {GameId} hit and sunk a ship.", coordinate, gameId);
            return Result<GameShotPostResponse>.Success(GameShotPostResponse.Map(ShotResult.Sunk));
        }

        _logger.LogInformation("Shot at {Coordinate} in game {GameId} was a hit.", coordinate, gameId);
        return Result<GameShotPostResponse>.Success(GameShotPostResponse.Map(ShotResult.Hit));
    }
}

/// <summary>
/// Provides extension methods for the <see cref="Game"/> class.
/// </summary>
file static class GameExtensions
{
    private const int BoardColumns = 10;
    private const int BoardRows = 10;
    private const int MinRow = 1;
    private const int MinColIndex = 0;
    private const int MinInclusive = 0;
    private const int MaxExclusive = 2;
    private const char CharA = 'A';

    /// <summary>
    /// Attempts to place a ship of the specified type on the game board at a random position and orientation.
    /// </summary>
    /// <param name="game">The game instance.</param>
    /// <param name="type">The type of ship to place.</param>
    /// <returns><c>true</c> if the ship was placed successfully; otherwise, <c>false</c> if a collision occurred.</returns>
    public static bool TryPlaceShip(this Game game, ShipType type)
    {
        int length = (int)type;
        bool horizontal = RandomNumberGenerator
            .GetInt32(MaxExclusive) is MinInclusive;

        int maxColIndex = horizontal
            ? BoardColumns - length
            : BoardColumns - 1;

        int maxRowIndex = horizontal
            ? BoardRows - 1
            : BoardRows - length;

        int colStart = RandomNumberGenerator.GetInt32(MinColIndex, maxColIndex + 1);
        int rowStart = RandomNumberGenerator.GetInt32(MinRow, maxRowIndex + 1);

        var positions = Enumerable.Range(0, length)
            .Select(i => Coordinate.Create(
                (char)(CharA + (horizontal ? colStart + i : colStart)),
                horizontal ? rowStart : rowStart + i
            ))
            .ToList();

        bool collision = game.Ships
            .SelectMany(s => s.Positions)
            .Intersect(positions, new CoordinateComparer())
            .Any();

        if (collision)
            return false;

        game.AddShip(Ship.Create(type, positions));
        return true;
    }
}