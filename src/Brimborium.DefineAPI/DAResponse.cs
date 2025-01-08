namespace Brimborium.DefineAPI;

public class DAResponse {
    public string MetaType { get; set; } = string.Empty;
    public string ETag { get; set; } = string.Empty;
}

public class DAResponse<T> : DAResponse {
    public T Payload { get; set; } = default!;
}
