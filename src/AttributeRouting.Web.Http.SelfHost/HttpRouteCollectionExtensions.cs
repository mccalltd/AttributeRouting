using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using AttributeRouting.Framework;
using AttributeRouting.Web.Http.SelfHost.Framework;
using AttributeRouting.Web.Http.SelfHost.Framework.Factories;

namespace AttributeRouting.Web.Http.SelfHost
{
    /// <summary>
    /// Extensions to the HttpRouteCollection
    /// </summary>
    public static class HttpRouteCollectionExtensions
    {
        /// <summary>
        /// Scans the calling assembly for all routes defined with AttributeRouting attributes,
        /// using the default conventions.
        /// </summary>
        public static void MapHttpAttributeRoutes(this HttpRouteCollection routes)
        {
            var configuration = new HttpAttributeRoutingConfiguration();
            configuration.ScanAssembly(Assembly.GetCallingAssembly());

            routes.MapHttpAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="configurationAction">
        /// The initialization action that builds the configuration object.
        /// </param>
        public static void MapHttpAttributeRoutes(this HttpRouteCollection routes, Action<HttpAttributeRoutingConfiguration> configurationAction)
        {
            var configuration = new HttpAttributeRoutingConfiguration();
            configurationAction.Invoke(configuration);

            routes.MapHttpAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="configuration">
        /// The configuration object.
        /// </param>
        public static void MapHttpAttributeRoutes(this HttpRouteCollection routes, HttpAttributeRoutingConfiguration configuration)
        {
            routes.MapHttpAttributeRoutesInternal(configuration);
        }

        private static void MapHttpAttributeRoutesInternal(this HttpRouteCollection routes, HttpAttributeRoutingConfiguration configuration)
        {
            var generatedRoutes = RouteBuilderFactory.Create(configuration).BuildAllRoutes();

            generatedRoutes.ToList().ForEach(r => routes.Add(r.RouteName, r.Route));
        }
    }
}