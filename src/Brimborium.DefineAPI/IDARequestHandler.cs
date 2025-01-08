namespace Brimborium.DefineAPI;

public interface IDARequestHandler {
    string? GetETagOfObject(object payload);

    string GetMetaType();
}

public interface IDARequestHandler<T> : IDARequestHandler {
    string? GetETagOfPayload(T payload);
}

public interface IDARequestHandlerObject : IDARequestHandler {
}

public class DARequestHandlerDelegate(
    string metaType,
    Func<object, string?>? getETagOfObject = default
    ) : IDARequestHandler {
    public string? GetETagOfObject(object payload)
        => (getETagOfObject is { }) ? getETagOfObject(payload) : null;

    public string GetMetaType() => metaType;
}

public class DARequestHandler<T>(
    string metaType
    ) : IDARequestHandler<T> {
    public string? GetETagOfObject(object payload) => this.GetETagOfPayload((T)payload);

    public virtual string? GetETagOfPayload(T payload) {
        return null;
    }

    public virtual string GetMetaType() => metaType;
}

public class DARequestHandlerObject(
    string metaType
    ) : IDARequestHandlerObject {
    public virtual string? GetETagOfObject(object payload) => null;

    public string GetMetaType() => metaType;
}
