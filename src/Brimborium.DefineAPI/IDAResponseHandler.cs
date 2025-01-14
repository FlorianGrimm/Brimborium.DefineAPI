namespace Brimborium.DefineAPI;

public interface IDAResponseHandler {
    string? GetETagOfObject(object payload);

    string GetMetaType();
}
public interface IDAResponseHandler<T> : IDAResponseHandler {
    string? GetETagOfPayload(T payload);
}

public interface IDAResponseHandlerObject : IDAResponseHandler {
}

public class DAResponseHandlerDelegate<T>(
    string metaType,
    Func<T, string?>? getETagOfObject = default
    ) : IDAResponseHandler<T> {
    public string? GetETagOfObject(object payload)
        => (getETagOfObject is { }) ? GetETagOfPayload((T)payload) : null;

    public string? GetETagOfPayload(T payload)
        => (getETagOfObject is { }) ? getETagOfObject(payload) : null;

    public string GetMetaType() => metaType;
}

public class DAResponseHandler<T>(
    string metaType
    ) : IDAResponseHandler<T> {
    public string? GetETagOfObject(object payload)
        => (payload is T payloadT) ? this.GetETagOfPayload(payloadT) : null;

    public virtual string? GetETagOfPayload(T payload) {
        return null;
    }

    public string GetMetaType() => metaType;
}

public class DAResponseHandlerObject(
    string metaType
    ) : IDAResponseHandlerObject {
    public virtual string? GetETagOfObject(object payload) => null;

    public string GetMetaType() => metaType;
}
