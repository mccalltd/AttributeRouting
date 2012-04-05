using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using AttributeRouting.Http.SelfHost.Framework;
using AttributeRouting.Http.SelfHost.Framework.Factories;

namespace AttributeRouting.Http.SelfHost
{
    /// <summary>
    /// Extensions to the MVC RouteCollection.
    /// </summary>
    public static class RouteCollectionExtensions
    {
        /// <summary>
        /// Scans the calling assembly for all routes defined with AttributeRouting attributes,
        /// using the default conventions.
        /// </summary>
        public static void MapAttributeRoutes(this HttpRouteCollection routes)
        {
            var configuration = new HttpAttributeRoutingConfiguration();
            configuration.ScanAssembly(Assembly.GetCallingAssembly());

            routes.MapAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="configurationAction">
        /// The initialization action that builds the configuration object.
        /// </param>
        public static void MapAttributeRoutes(this HttpRouteCollection routes, Action<HttpAttributeRoutingConfiguration> configurationAction)
        {
            var configuration = new HttpAttributeRoutingConfiguration();
            configurationAction.Invoke(configuration);

            routes.MapAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="configuration">
        /// The configuration object.
        /// </param>
        public static void MapAttributeRoutes(this HttpRouteCollection routes, HttpAttributeRoutingConfiguration configuration)
        {
            routes.MapAttributeRoutesInternal(configuration);
        }

        private static void MapAttributeRoutesInternal(this HttpRouteCollection routes, HttpAttributeRoutingConfiguration configuration)
        {
            var generatedRoutes = new RouteBuilder(
                configuration, new AttributeRouteFactory(), new ConstraintFactory(), new RouteParameterFactory()).BuildAllRoutes();

            generatedRoutes.ToList().ForEach(r => routes.Add(r.RouteName, r.Route));
        }
    }
}