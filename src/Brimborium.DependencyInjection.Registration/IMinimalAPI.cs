using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Brimborium.DependencyInjection.Registration;

public interface IMinimalAPI {
    //public void MapEndpointsApp(IApplicationBuilder app) { }

    public void MapEndpoints(IApplicationBuilder app, IEndpointRouteBuilder endpoints) { }
}
