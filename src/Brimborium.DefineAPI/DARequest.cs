namespace Brimborium.DefineAPI;

public interface IDARequest {
    string Command { get; set; }

    string MetaType { get; set; }

    string? EtagIfNotChanged { get; set; }
}

public interface IDARequest<T>: IDARequest {
    T Payload { get; set; }
}

public interface IDARequestObject : IDARequest {
    object? Payload { get; set; }
}

public class DARequest : IDARequest {

    public string Command { get; set; } = string.Empty;

    public string MetaType { get; set; } = string.Empty;

    public string? EtagIfNotChanged { get; set; }

}

public class DARequest<T> : DARequest {
    public T Payload { get; set; } = default!;
}

public class DARequestObject : DARequest, IDARequestObject {
    public object? Payload { get; set; }
}

/*
 
     public static DARequest<T> Create<T>(T payload) {
        return new DARequest<T>() {
            Payload = payload
        };
    }

 */
