using Battleship.Application.Interfaces.Repositories;
using Battleship.Infrastructure.Data;
using Battleship.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Battleship.Infrastructure;

/// <summary>
/// Provides extension methods for registering infrastructure layer services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers infrastructure services, including the database context and repositories, into the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add the infrastructure services to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<GameDbContext>(options =>
            options.UseInMemoryDatabase("BattleshipDb"));

        services.AddScoped<IGameRepository, GameRepository>();

        return services;
    }
}