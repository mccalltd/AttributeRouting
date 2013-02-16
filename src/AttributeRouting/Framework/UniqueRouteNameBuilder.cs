using System.Collections.Generic;
using System.Linq;
using System.Text;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Strategy ensuring that every route has a unique name.
    /// Preferably, it generates routes names like "Area_Controller_Action".
    /// In case of duplicates, it will append the HTTP method (if not a GET route) and/or a unique index to the route.
    /// So the most heinous possible form is "Area_Controller_Action_Method_Index".
    /// </summary>
    public class UniqueRouteNameBuilder : IRouteNameBuilder
    {
        private readonly HashSet<string> _registeredRouteNames = new HashSet<string>();

        public string Execute(RouteSpecification routeSpec)
        {
            var routeNameBuilder = new StringBuilder();

            if (routeSpec.AreaName.HasValue())
                routeNameBuilder.AppendFormat("{0}_", routeSpec.AreaName);

            routeNameBuilder.Append(routeSpec.ControllerName);
            routeNameBuilder.AppendFormat("_{0}", routeSpec.ActionName);

            // Ensure route names are unique. 
            var routeName = routeNameBuilder.ToString();
            var routeNameIsRegistered = _registeredRouteNames.Contains(routeName);
            if (routeNameIsRegistered)
            {
                // Prefix with the first verb (assuming this is the primary verb) if not a GET route.
                if (routeSpec.HttpMethods.Length > 0 && !routeSpec.HttpMethods.Contains("GET"))
                {
                    routeNameBuilder.AppendFormat("_{0}", routeSpec.HttpMethods.FirstOrDefault());
                }

                // Suffixing with an index if necessary to disambiguate.
                routeName = routeNameBuilder.ToString();
                var count = _registeredRouteNames.Count(n => n == routeName || n.StartsWith(routeName + "_"));
                if (count > 0)
                {
                    routeNameBuilder.AppendFormat("_{0}", count);
                }
            }

            routeName = routeNameBuilder.ToString();

            _registeredRouteNames.Add(routeName);

            return routeName;
        }
    }
}