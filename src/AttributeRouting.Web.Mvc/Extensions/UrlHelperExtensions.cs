using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Helpers;

namespace AttributeRouting.Web.Mvc.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string SubdomainAction(this UrlHelper urlHelper, string actionName, string controllerName = null, object routeValues = null, string protocol = null)
        {
            var routeValueDictionary = new RouteValueDictionary(routeValues);
            var baseUrl = GetDomainBase(urlHelper, null, actionName, controllerName, routeValueDictionary, protocol);
            return BuildUri(baseUrl, urlHelper.Action(actionName, controllerName, routeValues));
        }

        public static string SubdomainRouteUrl(this UrlHelper urlHelper, object routeValues, string protocol = null)
        {
            return SubdomainRouteUrl(urlHelper, null, routeValues, protocol);
        }

        public static string SubdomainRouteUrl(this UrlHelper urlHelper, string routeName, object routeValues = null, string protocol = null)
        {
            var routeValueDictionary = new RouteValueDictionary(routeValues);
            var baseUrl = GetDomainBase(urlHelper, routeName, null, null, routeValueDictionary, protocol);
            return BuildUri(baseUrl, urlHelper.RouteUrl(routeName, routeValueDictionary));
        }

        /// <summary>
        /// Gets the domain base url.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="schema">The schema.</param>
        /// <returns></returns>
        private static string GetDomainBase(UrlHelper urlHelper, string routeName, string actionName, string controllerName, RouteValueDictionary routeValues, string schema = null)
        {
            // baseUrl is the return value which by default is an empty string.
            var baseUrl = String.Empty;

            // Just a shortcut variable so we don't have to have this the below line eight million times.
            var currentUrl = urlHelper.RequestContext.HttpContext.Request.Url;

            //get the desired route using a copy of MS internal methods
            var values = MergeRouteValues(actionName, controllerName, urlHelper.RequestContext.RouteData.Values, routeValues, true);
            var virtualPathForArea = urlHelper.RouteCollection.GetVirtualPathForArea(urlHelper.RequestContext, routeName, values);
            if (virtualPathForArea == null)
            {
                return baseUrl;
            }

            // If not an AttributeRoute or the current url is funny then nothing we can do so move on.
            var route = virtualPathForArea.Route as IAttributeRoute;
            if (route != null && currentUrl != null && currentUrl.OriginalString.HasValue())
            {
                // Get the current domain via the current Uri.
                var host = currentUrl.GetLeftPart(UriPartial.Authority).Replace(currentUrl.GetLeftPart(UriPartial.Scheme), string.Empty);

                // If the port exists in the host remove it so that we don't run into trouble with the IPAddress parsing
                IPAddress ip;
                if (host.Contains(":"))
                {
                    host = host.Substring(0, host.IndexOf(":", StringComparison.Ordinal));
                }

                // If an ip then no point in building a subdomain for it
                if (IPAddress.TryParse(host, out ip))
                {
                    return string.Empty;
                }

                // Save the current host for comparisons later
                var currentHost = host;

                // Which protocol schema to use. i.e. http, https
                var scheme = schema ?? currentUrl.Scheme;

                // What is the current port. needed if non-standard
                var port = currentUrl.Port;

                // Is the port a standard port?
                var useDefaultPort = port == 80 || port == 443;

                // Need the default subdomain incase we are going from one subdomain method to a non-subdomain method
                var defaultSubdomain = String.Empty;
                object defaultSubdomainValue;
                if (route.DataTokens.TryGetValue("defaultSubdomain", out defaultSubdomainValue))
                {
                    defaultSubdomain = defaultSubdomainValue.ToString();
                }

                // If the host contains a dot we need to remove the subdomain if it is in the list of ones to remove
                if (host.Contains("."))
                {
                    // Get all registered subdomains
                    var subdomains = urlHelper.RouteCollection
                                              .Where(x => x is IAttributeRoute)
                                              .Cast<IAttributeRoute>()
                                              .Where(x => x.Subdomain.HasValue())
                                              .Select(x => x.Subdomain)
                                              .Distinct()
                                              .ToList();

                    // Also add the default subdomain from the current route
                    if (!string.IsNullOrWhiteSpace(defaultSubdomain) && !subdomains.Contains(defaultSubdomain))
                    {
                        subdomains.Add(defaultSubdomain);
                    }

                    // Strips subdomain information off of current (if matching a current one)
                    var subdomainSection = host.Split('.')[0];
                    var subdomain = subdomains.FirstOrDefault(subdomainSection.ValueEquals);
                    if (subdomain.HasValue())
                    {
                        host = host.Replace(subdomain + ".", null);
                    }
                }

                // If not a subdomain then don't build the url; instead build it to the default subdomain.
                if (route.Subdomain.HasValue())
                {
                    // If host already starts with subdomain then skip building the url
                    if (!currentHost.StartsWith(route.Subdomain))
                    {
                        baseUrl = "{0}://{1}.{2}".FormatWith(scheme, route.Subdomain, host);
                    }
                }
                else
                {
                    // No subdomain so we should add the default subdomain unless it is localhost 
                    var subdomain = string.Empty;
                    if (!host.ValueEquals("localhost") && defaultSubdomain.HasValue())
                    {
                        subdomain = defaultSubdomain + ".";
                    }
                    baseUrl = "{0}://{1}{2}".FormatWith(scheme, subdomain, host);
                }

                //not using a standard port so if the baseurl has a value then append on the port
                if (baseUrl.HasValue() && !useDefaultPort)
                {
                    baseUrl = "{0}:{1}".FormatWith(baseUrl, port);
                }
            }

            return baseUrl;
        }

        private static string BuildUri(string baseUrl, string relativeUrl)
        {
            if (baseUrl.HasNoValue() && relativeUrl.HasNoValue())
            {
                return String.Empty;
            }
            if (baseUrl.HasNoValue())
            {
                return relativeUrl;
            }
            if (relativeUrl.HasNoValue())
            {
                return baseUrl;
            }
            if (!relativeUrl.StartsWith("/"))
            {
                relativeUrl = "/" + relativeUrl;
            }
            return "{0}{1}".FormatWith(baseUrl, relativeUrl);
        }

        public static RouteValueDictionary MergeRouteValues(string actionName, string controllerName, RouteValueDictionary implicitRouteValues, RouteValueDictionary routeValues, bool includeImplicitMvcValues)
        {
            var routeValueDictionary = new RouteValueDictionary();
            if (includeImplicitMvcValues)
            {
                object obj;
                if (implicitRouteValues != null && implicitRouteValues.TryGetValue("action", out obj))
                {
                    routeValueDictionary["action"] = obj;
                }
                if (implicitRouteValues != null && implicitRouteValues.TryGetValue("controller", out obj))
                {
                    routeValueDictionary["controller"] = obj;
                }
            }
            if (routeValues != null)
            {
                var newRouteValueDictionary = new RouteValueDictionary(routeValues);
                foreach (var keyValuePair in newRouteValueDictionary)
                {
                    routeValueDictionary[keyValuePair.Key] = keyValuePair.Value;
                }
            }
            if (actionName != null)
            {
                routeValueDictionary["action"] = actionName;
            }
            if (controllerName != null)
            {
                routeValueDictionary["controller"] = controllerName;
            }
            return routeValueDictionary;
        }
    }
}