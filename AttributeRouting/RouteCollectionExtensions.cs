using System;
using System.Linq;
using System.Reflection;
using System.Web.Routing;

namespace AttributeRouting
{
    public static class RouteCollectionExtensions
    {
        public static void MapAttributeRoutes(this RouteCollection routes)
        {
            var configuration = new AttributeRoutingConfiguration();
            configuration.ScanAssembly(Assembly.GetCallingAssembly());

            routes.MapAttributeRoutesInternal(configuration);
        }

        public static void MapAttributeRoutes(this RouteCollection routes, Action<AttributeRoutingConfiguration> configurationAction)
        {
            var configuration = new AttributeRoutingConfiguration();
            configurationAction.Invoke(configuration);

            routes.MapAttributeRoutesInternal(configuration);
        }

        public static void MapAttributeRoutes(this RouteCollection routes, AttributeRoutingConfiguration configuration)
        {
            routes.MapAttributeRoutesInternal(configuration);
        }

        private static void MapAttributeRoutesInternal(this RouteCollection routes, AttributeRoutingConfiguration configuration)
        {
            var generatedRoutes = new RouteBuilder(configuration).BuildAllRoutes();
            
            generatedRoutes.ToList().ForEach(r => routes.Add(r.Name, r));
        }
    }
}