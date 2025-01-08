namespace Brimborium.DefineAPI;

public interface IDAResponseHandler {
}
public interface IDAResponseHandler<T> : IDAResponseHandler {
}

public interface IDAResponseHandlerObject : IDAResponseHandler {
}

public class DAResponseHandler : IDAResponseHandler {
}

public class DAResponseHandler<T> : DAResponseHandler, IDAResponseHandler<T> {
}

public class DAResponseHandlerObject : DAResponseHandler, IDAResponseHandlerObject {
}
