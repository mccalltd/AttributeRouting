using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Web.Http.WebHost.Framework;

namespace AttributeRouting.Web.Http.WebHost
{
    /// <summary>
    /// Extensions to the System.Web.Routing.RouteCollection.
    /// </summary>
    public static class RouteCollectionExtensions
    {
        /// <summary>
        /// Scans the calling assembly for all routes defined with AttributeRouting attributes,
        /// using the default conventions.
        /// </summary>
        public static void MapHttpAttributeRoutes(this RouteCollection routes)
        {
            var configuration = new HttpWebAttributeRoutingConfiguration();
            configuration.ScanAssembly(Assembly.GetCallingAssembly());

            routes.MapAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="routes"> </param>
        /// <param name="configurationAction">The initialization action that builds the configuration object</param>
        public static void MapHttpAttributeRoutes(this RouteCollection routes, Action<HttpWebAttributeRoutingConfiguration> configurationAction)
        {
            var configuration = new HttpWebAttributeRoutingConfiguration();
            configurationAction.Invoke(configuration);

            routes.MapAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="routes"> </param>
        /// <param name="configuration">The configuration object</param>
        public static void MapHttpAttributeRoutes(this RouteCollection routes, HttpWebAttributeRoutingConfiguration configuration)
        {
            routes.MapAttributeRoutesInternal(configuration);
        }

        private static void MapAttributeRoutesInternal(this RouteCollection routes, HttpWebAttributeRoutingConfiguration configuration)
        {
            var generatedRoutes = new RouteBuilder(configuration).BuildAllRoutes().ToList();
            var globalConfigRoutes = GlobalConfiguration.Configuration.Routes;

            foreach (var attributeRoute in generatedRoutes)
            {
                var routeTemplate = attributeRoute.Url;
                var defaults = new HttpRouteValueDictionary(attributeRoute.Defaults);
                var constraints = new HttpRouteValueDictionary(attributeRoute.Constraints);
                var dataTokens = new HttpRouteValueDictionary(attributeRoute.DataTokens);
                var httpRoute = globalConfigRoutes.CreateRoute(routeTemplate, defaults, constraints, dataTokens);

                var route = (HttpWebAttributeRoute)attributeRoute;
                route.HttpRoute = httpRoute;
                routes.Add(route.RouteName, route);
            }

            //generatedRoutes.ToList().ForEach(r => routes.Add(r.RouteName, (Route)r));
        }
    }
}