namespace Battleship.Api;    

/// <summary>
/// Provides extension methods for registering API-related services and configurations.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds API-related services to the specified <see cref="IServiceCollection"/>.
    /// Configures CORS policies, exception handling, API explorer, and Swagger generation.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The application configuration instance.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddCorsPolicies(configuration)
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        return services;
    }

    /// <summary>
    /// Adds and configures CORS policies using the specified <see cref="IConfiguration"/>.
    /// </summary>
    /// <param name="services">The service collection to add CORS policies to.</param>
    /// <param name="configuration">The application configuration instance.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    private static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
    {
        string[] allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }
}