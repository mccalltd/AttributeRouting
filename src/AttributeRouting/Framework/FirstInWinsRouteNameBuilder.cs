using System.Collections.Generic;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Strategy that generates routes in the form "Area_Controller_Action". 
    /// In case of duplicates, the duplicate route is not named, and the builder will return null.
    /// </summary>
    public class FirstInWinsRouteNameBuilder : IRouteNameBuilder
    {
        private readonly HashSet<string> _registeredRouteNames = new HashSet<string>();

        public string Execute(RouteSpecification routeSpec)
        {
            var areaPart = routeSpec.AreaName.HasValue() ? "{0}_".FormatWith(routeSpec.AreaName) : null;
            var routeName = "{0}{1}_{2}".FormatWith(areaPart, routeSpec.ControllerName, routeSpec.ActionName);

            // Only register route names once, so first in wins.
            if (!_registeredRouteNames.Contains(routeName))
            {
                _registeredRouteNames.Add(routeName);
                return routeName;
            }

            return null;
        }
    }
}