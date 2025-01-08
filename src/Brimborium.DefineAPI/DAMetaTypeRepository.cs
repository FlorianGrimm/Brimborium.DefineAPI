using System.Diagnostics.CodeAnalysis;

namespace Brimborium.DefineAPI;

public class DAMetaTypeRepository {
    public Dictionary<Type, IDARequestHandler> _RequestHandlerByType = new();
    public Dictionary<string, IDAResponseHandler> _ResponseHandlerByMetaType = new(StringComparer.Ordinal);

    public void RegisterRequestHandler<T>(T requestHandler)
        where T : IDARequestHandler<T> {
        this.RegisterRequestHandler(typeof(T), requestHandler);
    }

    public void RegisterRequestHandler(Type requestType, IDARequestHandler requestHandler) {
        if (this._RequestHandlerByType.TryAdd(requestType, requestHandler)) {
            return;
        }
        var existing = this._RequestHandlerByType[requestType];
        if (ReferenceEquals(requestHandler, existing)) {
            return;
        }
        throw new ArgumentException("requestHandler already exists.", nameof(requestType));
    }

    public bool TryGetRequestHandler(Type typePayload, [MaybeNullWhen(false)] out IDARequestHandler requestHandler) {
        if (this._RequestHandlerByType.TryGetValue(typePayload, out requestHandler)) {
            return true;
        }
        requestHandler = default;
        return false;
    }

    public void RegisterResponseHandler<T>(T responseHandler)
        where T : IDAResponseHandler<T> {
        this.RegisterResponseHandler(typeof(T), responseHandler);
    }

    public void RegisterResponseHandler(Type typePayload, IDAResponseHandler responseHandler) {
    }

    public bool TryGetResponseHandler(Type typePayload, [MaybeNullWhen(false)] out IDAResponseHandler responseHandler) {
        responseHandler = default;
        return false;
    }

    public string? GetMetaType(Type typePayload) {
        return null;
    }

    public DARequest<T> CreateDARequestT<T>(T payload, string? etag) {
        var actualType = payload?.GetType();
        var definedType = typeof(T);
        IDARequestHandler? requestHandler;
        if (actualType is null) {
            if (!this.TryGetRequestHandler(definedType, out requestHandler)) {
                throw new ArgumentException("Type of Payload is unknown", nameof(payload));
            }
            if (etag is null) {
                etag = requestHandler.GetETagOfObject(payload);
            }
        } else {
            if (!this.TryGetRequestHandler(actualType, out requestHandler)) {
                if (!this.TryGetRequestHandler(definedType, out requestHandler)) {
                    throw new ArgumentException("Type of Payload is unknown", nameof(payload));
                }
            }
            if (etag is null) {
                etag = requestHandler.GetETagOfObject(payload);
            }
        }

        var result = new DARequest<T>();
        result.MetaType = requestHandler.GetMetaType();
        result.Payload = payload;
        result.EtagIfNotChanged = etag;
        return result;
    }
}

