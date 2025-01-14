using System.Diagnostics.CodeAnalysis;

namespace Brimborium.DefineAPI;

public class DAMetaTypeRepository {
    public Dictionary<Type, IDARequestHandler> _RequestHandlerByType = new();
    public Dictionary<string, IDARequestHandler> _RequestHandlerByMetaType = new();

    public Dictionary<Type, IDAResponseHandler> _ResponseHandlerByType = new();
    public Dictionary<string, IDAResponseHandler> _ResponseHandlerByMetaType = new(StringComparer.Ordinal);

    public DAMetaTypeRepository() {
    }

    public DAMetaTypeRepository(IEnumerable<IDAMetaTypeRepositoryBuilder> builders) {
        foreach (var builder in builders) {
            builder.Register(this);
        }
    }

    public void RegisterRequestHandlerTyped<T>(string metaType, IDARequestHandler<T> requestHandler) {
        Type typePayload = typeof(T);
        if (this._RequestHandlerByType.TryGetValue(typePayload, out var existingByType)) {
            if (ReferenceEquals(requestHandler, existingByType)) {
                return;
            } else {
                throw new ArgumentException("requestHandler with typePayload already exists.", nameof(requestHandler));
            }
        }
        if (this._RequestHandlerByMetaType.TryGetValue(metaType, out var existingByMetaType)) {
            // idempotent is ok
            if (ReferenceEquals(requestHandler, existingByMetaType)) {
                return;
            } else {
                throw new ArgumentException("requestHandler with metaType already exists.", nameof(requestHandler));
            }
        }
        this._RequestHandlerByType.Add(typePayload, requestHandler);
        this._RequestHandlerByMetaType.Add(metaType, requestHandler);
    }


    public void RegisterRequestHandler(Type typePayload, string metaType, IDARequestHandler requestHandler) {
        if (this._RequestHandlerByType.TryGetValue(typePayload, out var existingByType)) {
            if (ReferenceEquals(requestHandler, existingByType)) {
                return;
            } else {
                throw new ArgumentException("requestHandler with typePayload already exists.", nameof(requestHandler));
            }
        }
        if (this._RequestHandlerByMetaType.TryGetValue(metaType, out var existingByMetaType)) {
            // idempotent is ok
            if (ReferenceEquals(requestHandler, existingByMetaType)) {
                return;
            } else {
                throw new ArgumentException("requestHandler with metaType already exists.", nameof(requestHandler));
            }
        }
        this._RequestHandlerByType.Add(typePayload, requestHandler);
        this._RequestHandlerByMetaType.Add(metaType, requestHandler);
    }


    public bool TryGetRequestHandler(Type typePayload, [MaybeNullWhen(false)] out IDARequestHandler requestHandler) {
        if (this._RequestHandlerByType.TryGetValue(typePayload, out requestHandler)) {
            return true;
        }
        requestHandler = default;
        return false;
    }


    public void RegisterResponseHandlerTyped<T>(string metaType, IDAResponseHandler<T> responseHandler) {
        Type typePayload = typeof(T);
        if (this._ResponseHandlerByType.TryGetValue(typePayload, out var existingByType)) {
            if (ReferenceEquals(responseHandler, existingByType)) {
                return;
            } else {
                throw new ArgumentException("responseHandler with typePayload already exists.", nameof(responseHandler));
            }
        }
        if (this._ResponseHandlerByMetaType.TryGetValue(metaType, out var existingByMetaType)) {
            // idempotent is ok
            if (ReferenceEquals(responseHandler, existingByMetaType)) {
                return;
            } else {
                throw new ArgumentException("responseHandler with metaType already exists.", nameof(responseHandler));
            }
        }
        this._ResponseHandlerByType.Add(typePayload, responseHandler);
        this._ResponseHandlerByMetaType.Add(metaType, responseHandler);
    }

    public void RegisterResponseHandler(Type typePayload, string metaType, IDAResponseHandler responseHandler) {
        if (this._ResponseHandlerByType.TryGetValue(typePayload, out var existingByType)) {
            if (ReferenceEquals(responseHandler, existingByType)) {
                return;
            } else {
                throw new ArgumentException("responseHandler with typePayload already exists.", nameof(responseHandler));
            }
        }
        if (this._ResponseHandlerByMetaType.TryGetValue(metaType, out var existingByMetaType)) {
            // idempotent is ok
            if (ReferenceEquals(responseHandler, existingByMetaType)) {
                return;
            } else {
                throw new ArgumentException("responseHandler with metaType already exists.", nameof(responseHandler));
            }
        }
        this._ResponseHandlerByType.Add(typePayload, responseHandler);
        this._ResponseHandlerByMetaType.Add(metaType, responseHandler);
    }

    public bool TryGetResponseHandler(string metaType, [MaybeNullWhen(false)] out IDAResponseHandler responseHandler) {
        if (this._ResponseHandlerByMetaType.TryGetValue(metaType, out responseHandler)) {
            return true;
        }
        responseHandler = default;
        return false;
    }


    public DARequest<T> CreateDARequestT<T>(T payload, string? etag=default) {
        // avoid boxing
        var actualType = payload?.GetType();
        var definedType = typeof(T);
        IDARequestHandler? requestHandler;
        if (actualType is null) {
            if (!this._RequestHandlerByType.TryGetValue(definedType, out requestHandler)) {
                throw new ArgumentException("Type of Payload is unknown", nameof(payload));
            }
            /*
            if (etag is null) {
                etag = requestHandler.GetETagOfObject(payload);
            }
            */
        } else {
            if (!this._RequestHandlerByType.TryGetValue(actualType, out requestHandler)) {
                if (!this._RequestHandlerByType.TryGetValue(definedType, out requestHandler)) {
                    throw new ArgumentException("Type of Payload is unknown", nameof(payload));
                }
            }
            // actualType is not null so payload is not null
            if (etag is null) {
                if (requestHandler is IDARequestHandler<T> requestHandlerT) {
                    etag = requestHandlerT.GetETagOfPayload(payload!);
                } else {
                    etag = requestHandler.GetETagOfObject(payload!);
                }
            }
        }

        var result = new DARequest<T>();
        result.MetaType = requestHandler.GetMetaType();
        result.Payload = payload;
        result.EtagIfNotChanged = etag;
        return result;
    }


    public DAResponse<T> CreateDAResponse<T>(T payload, string? etag = default) {
        // avoid boxing
        var actualType = payload?.GetType();
        var definedType = typeof(T);
        IDAResponseHandler? responseHandler;
        if (actualType is null) {
            if (!this._ResponseHandlerByType.TryGetValue(definedType, out responseHandler)) {
                throw new ArgumentException("Type of Payload is unknown", nameof(payload));
            }
            /*
            if (etag is null) {
                etag = responseHandler.GetETagOfObject(payload);
            }
            */
        } else {
            if (!this._ResponseHandlerByType.TryGetValue(actualType, out responseHandler)) {
                if (!this._ResponseHandlerByType.TryGetValue(definedType, out responseHandler)) {
                    throw new ArgumentException("Type of Payload is unknown", nameof(payload));
                }
            }
            // actualType is not null so payload is not null
            if (etag is null) {
                if (responseHandler is IDAResponseHandler<T> responseHandlerT) {
                    etag = responseHandlerT.GetETagOfPayload(payload!);
                } else {
                    etag = responseHandler.GetETagOfObject(payload!);
                }
            }
        }

        var result = new DAResponse<T>();
        result.MetaType = responseHandler.GetMetaType();
        result.Payload = payload;
        result.ETag = etag;
        return result;
    }

}

public interface IDAMetaTypeRepositoryBuilder {
    void Register(DAMetaTypeRepository repository);
}