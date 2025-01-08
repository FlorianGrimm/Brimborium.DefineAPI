using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

using System.Diagnostics.CodeAnalysis;

namespace Brimborium.DependencyInjection.Registration;

public class MinimalAPIAppBuilder {
    public WebApplication App { get; }
    public Dictionary<string, RouteGroupBuilder> RouteGroupsByFullPath { get; } = new ();
    public Dictionary<RouteGroupBuilder, string> RouteGroupsByInstance { get; } = new ();

    public List<Action<string, RouteGroupBuilder>> ListActionGroup { get; } = [];

    public MinimalAPIAppBuilder(WebApplication app) {
        this.App = app;
    }

    public RouteGroupBuilder MapGroup(
        [StringSyntax("Route")] string prefix) {
        if (this.RouteGroupsByFullPath.TryGetValue(prefix, out var routeGroup)) {
            return routeGroup;
        } else {
            routeGroup = this.App.MapGroup(prefix).WithOpenApi();
            this.RouteGroupsByFullPath.Add(prefix, routeGroup);
            this.RouteGroupsByInstance.Add(routeGroup, prefix);
            return routeGroup;
        }
    }
    public RouteGroupBuilder MapGroup(
        RouteGroupBuilder parent,
        [StringSyntax("Route")] string prefix) {
        string fullPath;
        if (this.RouteGroupsByInstance.TryGetValue(parent, out var parentPrefix)) {
            fullPath = $"{parentPrefix}/{prefix}";
        } else {
            throw new InvalidOperationException("parent not found");
        }

        if (this.RouteGroupsByFullPath.TryGetValue(prefix, out var routeGroup)) {
            return routeGroup;
        } else {
            routeGroup = this.App.MapGroup(prefix).WithOpenApi();
            this.RouteGroupsByFullPath.Add(fullPath, routeGroup);
            this.RouteGroupsByInstance.Add(routeGroup, fullPath);
            return routeGroup;
        }
    }

    public void PostConfigure(Action<string, RouteGroupBuilder> action) {
        this.ListActionGroup.Add(action);
    }

    public void RunPostConfigure() {
        foreach (var action in this.ListActionGroup) {
            foreach (var (fullPath, routeGroup) in this.RouteGroupsByFullPath) {
                action(fullPath, routeGroup);
            }
        }
    }
}