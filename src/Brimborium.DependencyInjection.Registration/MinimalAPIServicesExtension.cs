using Brimborium.DependencyInjection.Registration;

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for adding minimal API services to the service collection.
/// </summary>
public static class MinimalAPIServicesExtension {

    /// <summary>
    /// Adds a minimal API service to the service collection.
    /// </summary>
    /// <typeparam name="T">The type of the minimal API service to add.</typeparam>
    /// <param name="services">The service collection to add the minimal API service to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMinimalAPI<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(
        this IServiceCollection services)
        where T : class, IMinimalAPI {
        services.AddTransient<IMinimalAPI>(static (IServiceProvider serviceProvider) => serviceProvider.GetRequiredService<T>());
        services.AddSingleton<T, T>();
        return services;
    }
}
