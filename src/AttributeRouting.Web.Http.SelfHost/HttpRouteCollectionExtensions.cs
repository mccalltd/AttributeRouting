using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using AttributeRouting.Framework;
using AttributeRouting.Web.Http.Framework;

namespace AttributeRouting.Web.Http.SelfHost
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
            var configuration = new HttpSelfHostRouteConfiguration();
            configuration.AddRoutesFromAssembly(Assembly.GetCallingAssembly());

            routes.MapHttpAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="configurationAction">The initialization action that builds the configuration object</param>
        public static void MapHttpAttributeRoutes(this HttpRouteCollection routes, Action<HttpSelfHostRouteConfiguration> configurationAction)
        {
            var configuration = new HttpSelfHostRouteConfiguration();
            configurationAction.Invoke(configuration);
            
            routes.MapHttpAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="routes"> </param>
        /// <param name="configuration">The configuration object</param>
        public static void MapHttpAttributeRoutes(this HttpRouteCollection routes, HttpSelfHostRouteConfiguration configuration)
        {
            routes.MapHttpAttributeRoutesInternal(configuration);
        }

        private static void MapHttpAttributeRoutesInternal(this HttpRouteCollection routes, HttpSelfHostRouteConfiguration configuration)
        {
            new RouteBuilder(configuration).BuildAllRoutes()
                                           .Cast<HttpAttributeRoute>()
                                           .ToList()
                                           .ForEach(r => routes.Add(r.RouteName, r));
        }
    }
}