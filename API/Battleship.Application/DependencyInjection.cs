using Battleship.Application.Interfaces.Services;
using Battleship.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Battleship.Application;

/// <summary>
/// Provides extension methods for registering application layer services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers application services and dependencies in the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GameService>();

        return services;
    }
}