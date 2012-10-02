using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Route name builders for generating conventional route names.
    /// </summary>
    public static class RouteNameBuilders
    {
        /// <summary>
        /// This builder ensures that every route has a unique name.
        /// Preferably, it generates routes names like "Area_Controller_Action".
        /// In case of duplicates, it will append the HTTP method (if not a GET route) and/or a unique index to the route.
        /// So the most heinous possible form is "Area_Controller_Action_Method_Index".
        /// </summary>
        public static Func<RouteSpecification, string> Unique
        {
            get { return CreateUniqueRouteNameBuilder(); }
        }

        /// <summary>
        /// This builder generates routes in the form "Area_Controller_Action". 
        /// In case of duplicates, the duplicate route is not named, and the builder will return null.
        /// </summary>
        public static Func<RouteSpecification, string> FirstInWins
        {
            get { return CreateFirstInWinsRouteNameBuilder(); }
        }

        private static Func<RouteSpecification, string> CreateUniqueRouteNameBuilder()
        {
            var registeredRouteNames = new List<string>();

            return routeSpec =>
            {
                var routeNameBuilder = new StringBuilder();

                if (routeSpec.AreaName.HasValue())
                    routeNameBuilder.AppendFormat("{0}_", routeSpec.AreaName);

                routeNameBuilder.Append(routeSpec.ControllerName);
                routeNameBuilder.AppendFormat("_{0}", routeSpec.ActionName);

                // Ensure route names are unique. 
                var routeName = routeNameBuilder.ToString();
                var routeNameIsRegistered = registeredRouteNames.Any(name => name == routeName);
                if (routeNameIsRegistered)
                {
                    // Prefix with the first verb (assuming this is the primary verb) if not a GET route.
                    if (routeSpec.HttpMethods.Length > 0 && !routeSpec.HttpMethods.Contains("GET"))
                        routeNameBuilder.AppendFormat("_{0}", routeSpec.HttpMethods.FirstOrDefault());

                    // Suffixing with an index if necessary to disambiguate.
                    routeName = routeNameBuilder.ToString();
                    var count = registeredRouteNames.Count(n => n == routeName || n.StartsWith(routeName + "_"));
                    if (count > 0)
                        routeNameBuilder.AppendFormat("_{0}", count);
                }

                routeName = routeNameBuilder.ToString();

                registeredRouteNames.Add(routeName);

                return routeName;
            };
        }

        private static Func<RouteSpecification, string> CreateFirstInWinsRouteNameBuilder()
        {
            var registeredRouteNames = new List<string>();

            return routeSpec =>
            {
                var areaPart = routeSpec.AreaName.HasValue() ? "{0}_".FormatWith(routeSpec.AreaName) : null;
                var routeName = "{0}{1}_{2}".FormatWith(areaPart, routeSpec.ControllerName, routeSpec.ActionName);

                // Only register route names once, so first in wins.
                if (!registeredRouteNames.Contains(routeName))
                {
                    registeredRouteNames.Add(routeName);
                    return routeName;
                }

                return null;
            };
        }
    }
}