using System;
using System.Collections.Generic;
using AttributeRouting.Helpers;

namespace AttributeRouting.Framework
{
    /// <summary>
    /// Visitor-ish used to extend implementations of <see cref="IAttributeRoute"/>.
    /// </summary>
    /// <remarks>
    /// Due to different route implementations in
    /// System.Web.Routing (used for MVC controller routes) and
    /// System.Web.Http.Routing (used for Web API controller routes). 
    /// </remarks>
    public class AttributeRouteVisitor
    {
        private readonly IAttributeRoute _route;
        private string _staticLeftPartOfUrl;

        /// <summary>
        /// Creates a new visitor extending implementations of IAttributeRoute with common logic.
        /// </summary>
        /// <param name="route">The route</param>
        public AttributeRouteVisitor(IAttributeRoute route)
        {
            if (route == null) throw new ArgumentNullException("route");

            _route = route;
        }
        
        private string StaticLeftPartOfUrl
        {
            get 
            {
                if (_staticLeftPartOfUrl == null)
                {
                    var routePath = _route.Url;
                    var indexOfFirstParam = routePath.IndexOf("{", StringComparison.OrdinalIgnoreCase);
                    var leftPart = (indexOfFirstParam == -1) ? routePath : routePath.Substring(0, indexOfFirstParam);
                    _staticLeftPartOfUrl = leftPart.TrimEnd('/');
                }
                return _staticLeftPartOfUrl;
            }
        }

        /// <summary>
        /// Adds querystring default values to route values collection if they aren't already present.
        /// </summary>
        /// <param name="routeValues">The route values.</param>
        public void AddQueryStringDefaultsToRouteValues(IDictionary<string, object> routeValues)
        {
            foreach (var queryStringDefault in _route.QueryStringDefaults)
            {
                // Don't add optional params.
                if (queryStringDefault.Value.HasNoValue())
                {
                    continue;
                }

                // Add default if no value is present in the current route values.
                if (!routeValues.ContainsKey(queryStringDefault.Key))
                {
                    routeValues.Add(queryStringDefault.Key, queryStringDefault.Value);
                }
            }            
        }

        /// <summary>
        /// Optimizes route matching by comparing the static left part of a route's URL with the requested path.
        /// </summary>
        /// <param name="requestedPath">The path of the requested URL.</param>
        /// <returns>True if the requested URL path starts with the static left part of the route's URL.</returns>
        /// <remarks>Thanks: http://samsaffron.com/archive/2011/10/13/optimising-asp-net-mvc3-routing </remarks>
        public bool IsStaticLeftPartOfUrlMatched(string requestedPath)
        {
            // Compare the left part with the requested path
            var comparableRequestedPath = requestedPath.TrimEnd('/');
            return comparableRequestedPath.StartsWith(StaticLeftPartOfUrl, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Processes query constraints separately from route constraints.
        /// </summary>
        /// <param name="processConstraint">
        /// Delegate used to process the query constraints according to the underlying route framework.
        /// Accepts a constraint and parameter name and returns true if the constraint passes.
        /// </param>
        /// <returns>True if all query string constraints pass or if there are none to test.</returns>
        /// <remarks>
        /// Need to separate path and query constraints because methods in the web stack
        /// will not add query params to generated urls if there is a constraint for the param name
        /// that is not present in the url template. See logic in:
        /// - System.Web.Http.Routing.HttpParsedRoute.Bind(...)
        /// - System.Web.Routing.ParsedRoute.Bind(...)
        /// </remarks>
        public bool ProcessQueryStringConstraints(Func<object, string, bool> processConstraint)
        {
            foreach (var queryStringConstraint in _route.QueryStringConstraints)
            {
                var parameterName = queryStringConstraint.Key;
                var constraint = queryStringConstraint.Value;

                if (!processConstraint(constraint, parameterName))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
