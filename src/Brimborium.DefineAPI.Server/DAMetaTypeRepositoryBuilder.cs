namespace Brimborium.DefineAPI.Server;

/// <summary>
/// Builder class for managing request and response handlers for DAMetaTypeRepository.
/// </summary>
public sealed class DAMetaTypeRepositoryBuilder : IDAMetaTypeRepositoryBuilder {
    /// <summary>
    /// Gets the list of request handlers.
    /// </summary>
    public List<RegistrationTypeRequestHandler> ListTypeRequestHandler { get; } = new();

    /// <summary>
    /// Gets the list of response handlers.
    /// </summary>
    public List<RegistrationTypeResponseHandler> ListTypeResponseHandler { get; } = new();

    /// <summary>
    /// Adds a request handler to the list.
    /// </summary>
    /// <typeparam name="T">The type of the request handler.</typeparam>
    /// <param name="value">The request handler instance.</param>
    /// <returns>fluent this</returns>
    public DAMetaTypeRepositoryBuilder AddRequestHandler<T>(string metaType, T value)
        where T : IDARequestHandler {
        this.ListTypeRequestHandler.Add(new RegistrationTypeRequestHandler(metaType, typeof(T), value));
        return this;
    }

    /// <summary>
    /// Adds a request handler to the list with a specified payload type.
    /// </summary>
    /// <typeparam name="T">The type of the request handler.</typeparam>
    /// <param name="typePayload">The type of the payload.</param>
    /// <param name="value">The request handler instance.</param>
    /// <returns>fluent this</returns>
    public DAMetaTypeRepositoryBuilder AddRequestHandler(string metaType, Type typePayload, IDARequestHandler value) {
        this.ListTypeRequestHandler.Add(new RegistrationTypeRequestHandler(metaType, typePayload, value));
        return this;
    }

    /// <summary>
    /// Adds a response handler to the list with a specified payload type.
    /// </summary>
    /// <typeparam name="T">The type of the response handler.</typeparam>
    /// <param name="typePayload">The type of the payload.</param>
    /// <param name="value">The response handler instance.</param>
    /// <returns>fluent this</returns>
    public DAMetaTypeRepositoryBuilder AddResponseHandle(string metaType, Type typePayload, IDAResponseHandler value) {
        this.ListTypeResponseHandler.Add(new RegistrationTypeResponseHandler(metaType, typePayload, value));
        return this;
    }

    void IDAMetaTypeRepositoryBuilder.Register(DAMetaTypeRepository repository) {
        foreach (var typeRequestHandler in this.ListTypeRequestHandler) {
            repository.RegisterRequestHandler(typeRequestHandler.TypePayload, typeRequestHandler.MetaType, typeRequestHandler.RequestHandler);
        }
        foreach (var typeResponseHandler in this.ListTypeResponseHandler) {
            repository.RegisterResponseHandler(typeResponseHandler.TypePayload, typeResponseHandler.MetaType, typeResponseHandler.ResponseHandler);
        }
    }
}

/// <summary>
/// Represents a request handler with its associated type.
/// </summary>
/// <param name="TypePayload">The type of the request handler.</param>
/// <param name="RequestHandler">The request handler instance.</param>
public record RegistrationTypeRequestHandler(string MetaType, Type TypePayload, IDARequestHandler RequestHandler);

/// <summary>
/// Represents a response handler with its associated type.
/// </summary>
/// <param name="MetaType">The MetaType of the response handler.</param>
/// <param name="ResponseHandler">The response handler instance.</param>
public record RegistrationTypeResponseHandler(string MetaType, Type TypePayload, IDAResponseHandler ResponseHandler);
