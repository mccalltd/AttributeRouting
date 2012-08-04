using System;
using System.Linq;
using System.Reflection;
using System.Web.Routing;
using AttributeRouting.Framework;

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
            var configuration = new HttpAttributeRoutingConfiguration();
            configuration.ScanAssembly(Assembly.GetCallingAssembly());

            routes.MapAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="routes"> </param>
        /// <param name="configurationAction">The initialization action that builds the configuration object</param>
        public static void MapHttpAttributeRoutes(this RouteCollection routes, Action<HttpAttributeRoutingConfiguration> configurationAction)
        {
            var configuration = new HttpAttributeRoutingConfiguration();
            configurationAction.Invoke(configuration);

            routes.MapAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="routes"> </param>
        /// <param name="configuration">The configuration object</param>
        public static void MapHttpAttributeRoutes(this RouteCollection routes, HttpAttributeRoutingConfiguration configuration)
        {
            routes.MapAttributeRoutesInternal(configuration);
        }

        private static void MapAttributeRoutesInternal(this RouteCollection routes, HttpAttributeRoutingConfiguration configuration)
        {
            var generatedRoutes = new RouteBuilder(configuration).BuildAllRoutes();

            generatedRoutes.ToList().ForEach(r => routes.Add(r.RouteName, (Route)r));
        }
    }
}