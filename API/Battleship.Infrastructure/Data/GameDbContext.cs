using Battleship.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Battleship.Infrastructure.Data;

/// <summary>
/// Represents the Entity Framework Core database context for Battleship games.
/// Provides access to the persisted <see cref="GameEntity"/> records.
/// </summary>
internal sealed class GameDbContext(
    DbContextOptions<GameDbContext> options
) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing Battleship games.
    /// </summary>
    public DbSet<GameEntity> Games { get; set; }
}