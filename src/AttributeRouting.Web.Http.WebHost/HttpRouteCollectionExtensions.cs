using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Web.Http.Framework;

namespace AttributeRouting.Web.Http.WebHost
{
    /// <summary>
    /// Extensions to the System.Web.Http.HttpRouteCollection
    /// </summary>
    public static class HttpRouteCollectionExtensions
    {
        /// <summary>
        /// Scans the calling assembly for all routes defined with AttributeRouting attributes,
        /// using the default conventions.
        /// </summary>
        public static void MapHttpAttributeRoutes(this HttpRouteCollection routes)
        {
            var configuration = new HttpWebConfiguration();
            configuration.AddRoutesFromAssembly(Assembly.GetCallingAssembly());

            routes.MapHttpAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="configurationAction">The initialization action that builds the configuration object</param>
        public static void MapHttpAttributeRoutes(this HttpRouteCollection routes, Action<HttpWebConfiguration> configurationAction)
        {
            var configuration = new HttpWebConfiguration();
            configurationAction.Invoke(configuration);

            if (configuration.InMemory)
            {
                var newConfig = new HttpWebConfiguration(inMemory: true);
                configurationAction.Invoke(newConfig);
                routes.MapHttpAttributeRoutesInternal(newConfig);
            }
            else
            {
                routes.MapHttpAttributeRoutesInternal(configuration);
            }
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="routes"> </param>
        /// <param name="configuration">The configuration object</param>
        public static void MapHttpAttributeRoutes(this HttpRouteCollection routes, HttpWebConfiguration configuration)
        {
            routes.MapHttpAttributeRoutesInternal(configuration);
        }

        private static void MapHttpAttributeRoutesInternal(this HttpRouteCollection routes, HttpWebConfiguration configuration)
        {
            new RouteBuilder(configuration).BuildAllRoutes()
                                           .Cast<HttpAttributeRoute>()
                                           .ToList()
                                           .ForEach(r => routes.Add(r.RouteName, r));
        }
    }
}