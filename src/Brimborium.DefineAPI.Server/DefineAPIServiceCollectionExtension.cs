using Brimborium.DefineAPI;
using Brimborium.DefineAPI.Server;

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for adding DefineAPI services to the IServiceCollection.
/// </summary>
public static class DefineAPIServiceCollectionExtension {

    /// <summary>
    /// Adds a DAMetaTypeRepository to the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection to add the repository to.</param>
    /// <param name="value">An optional DAMetaTypeRepository instance to be the service instance.</param>
    /// <returns>A DAMetaTypeRepositoryBuilder instance.</returns>
    public static DAMetaTypeRepositoryBuilder AddDefineAPI(
        this IServiceCollection serviceCollection,
        DAMetaTypeRepository? value = default
        ) {
        if (value is { }) {
            serviceCollection.AddSingleton<DAMetaTypeRepository>(value);
        } else {
            serviceCollection.TryAddSingleton<DAMetaTypeRepository>();
        }

        var result = new DAMetaTypeRepositoryBuilder();
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton<DAMetaTypeRepositoryBuilder>(result));
        return result;
    }
}
