using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Framework;

namespace AttributeRouting.Web.Mvc.Framework
{
    /// <summary>
    /// Route to use for ASP.NET MVC routes.
    /// </summary>
    public class AttributeRoute : Route, IAttributeRoute
    {
        private const string RequestedPathKey = "__AttributeRouting:RequestedPath";
        
        private readonly AttributeRouteVisitor _visitor;

        /// <summary>
        /// Route used by the AttributeRouting framework in web projects.
        /// </summary>
        public AttributeRoute(string url,
                              RouteValueDictionary defaults,
                              RouteValueDictionary constraints,
                              RouteValueDictionary dataTokens,
                              Configuration configuration)
            : base(url, defaults, constraints, dataTokens, configuration.RouteHandlerFactory())
        {
            _visitor = new AttributeRouteVisitor(this);
            QueryStringConstraints = new RouteValueDictionary();
            QueryStringDefaults = new RouteValueDictionary();
        }

        IDictionary<string, object> IAttributeRoute.Constraints
        {
            get { return Constraints; }
            set { Constraints = new RouteValueDictionary(value); }
        }

        IDictionary<string, object> IAttributeRoute.DataTokens
        {
            get { return DataTokens; }
            set { DataTokens = new RouteValueDictionary(value); }
        }

        IDictionary<string, object> IAttributeRoute.Defaults
        {
            get { return Defaults; }
            set { Defaults = new RouteValueDictionary(value); }
        }

        public IDictionary<string, object> QueryStringConstraints { get; set; }

        public IDictionary<string, object> QueryStringDefaults { get; set; }

        public string RouteName { get; set; }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            // Optimize matching by comparing the static left part of the route url with the requested path.
            var requestedPath = GetCachedValue(httpContext, RequestedPathKey, () => (httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo).TrimEnd('/'));
            if (!_visitor.IsStaticLeftPartOfUrlMatched(requestedPath))
            {
                return null;
            }

            // Let the underlying route match, and if it does, then add a few more constraints.
            var routeData = base.GetRouteData(httpContext);
            if (routeData == null)
            {
                return null;
            }

            // Constrain by querystring param if there are any.
            if (!_visitor.ProcessQueryStringConstraints((constraint, parameterName) => ProcessConstraint(httpContext, constraint, parameterName, routeData.Values, RouteDirection.IncomingRequest)))
            {
                return null;
            }

            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            // Add querystring default values if applicable.
            _visitor.AddQueryStringDefaultsToRouteValues(values);

            // Let the underlying route do its thing.
            var virtualPathData = base.GetVirtualPath(requestContext, values);
            if (virtualPathData == null)
            {
                return null;
            }

            return virtualPathData;
        }

        private static T GetCachedValue<T>(HttpContextBase context, object key, Func<T> initializeValue)
        {
            // Fetch the item from the http context if it's been stored for the request.
            if (context.Items.Contains(key))
            {
                return (T)context.Items[key];
            }

            // Cache the value and return it.
            var value = initializeValue();
            context.Items.Add(key, value);
            return value;
        }
    }
}
