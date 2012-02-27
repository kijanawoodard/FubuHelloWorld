using FubuMVC.Core;
using FubuMVC.Core.Diagnostics;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Spark;
using Scoretastic.Endpoints.Home;

namespace Scoretastic.Configuration
{
    public class ConfigureFubuMVC : FubuRegistry
    {
        public ConfigureFubuMVC()
        {
            // This line turns on the basic diagnostics and request tracing
            IncludeDiagnostics(true);

            Actions
                .IncludeTypesNamed(name => name.ToLower().EndsWith("endpoint"));

            // Policies
            Routes
                .HomeIs<HomeEndpoint>(e => e.Get(new HomeRequestModel()))
                //                .IgnoreControllerNamespaceEntirely()
                .UrlPolicy<EndpointUrlPolicy>()
                .ConstrainToHttpMethod(action => action.InputType().Name.Contains("Request"), "GET")
                .ConstrainToHttpMethod(action => action.InputType().Name.Contains("Command"), "POST");

            // Match views to action methods by matching
            // on model type, view name, and namespace
            Views
                .TryToAttachWithDefaultConventions();

            this.UseSpark();
        }
    }

    public class EndpointUrlPolicy : IUrlPolicy
    {
        public bool Matches(ActionCall call, IConfigurationObserver log)
        {
            return call.HandlerType.Name.EndsWith("Endpoint");
        }

        public IRouteDefinition Build(ActionCall call)
        {
            //Take the last part of the endpoint namespace and make that the route
            var routeDefinition = call.ToRouteDefinition();
            var space = call.HandlerType.Namespace ?? "the.world.is.a.strange.place";
            var index = space.LastIndexOf(".", System.StringComparison.Ordinal);
            var route = space.Substring(index + 1).ToLower();
            routeDefinition.Append(route);
            return routeDefinition;
        }
    }
}