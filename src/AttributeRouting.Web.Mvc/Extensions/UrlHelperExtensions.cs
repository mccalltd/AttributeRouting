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
        public static string SubdomainAction(this UrlHelper urlHelper, string actionName,
                                             RouteValueDictionary routeValues)
        {
            string baseUrl = GetDomainBase(urlHelper, null, actionName, null, routeValues);
            return BuildUri(baseUrl, urlHelper.Action(actionName, routeValues));
        }

        public static string SubdomainAction(this UrlHelper urlHelper, string actionName, string controllerName,
                                             string areaName = "")
        {
            var routeValues = new RouteValueDictionary { { "area", areaName } };
            string baseUrl = GetDomainBase(urlHelper, null, actionName, controllerName, routeValues);
            return BuildUri(baseUrl, urlHelper.Action(actionName, controllerName, routeValues));
        }

        public static string SubdomainAction(this UrlHelper urlHelper, string actionName, string controllerName,
                                             object routeValues)
        {
            string baseUrl = GetDomainBase(urlHelper, null, actionName, controllerName,
                                           new RouteValueDictionary(routeValues));
            return BuildUri(baseUrl, urlHelper.Action(actionName, controllerName, routeValues));
        }

        public static string SubdomainAction(this UrlHelper urlHelper, string actionName, string controllerName,
                                             RouteValueDictionary routeValues)
        {
            string baseUrl = GetDomainBase(urlHelper, null, actionName, controllerName, routeValues);
            return BuildUri(baseUrl, urlHelper.Action(actionName, controllerName, routeValues));
        }

        public static string SubdomainAction(this UrlHelper urlHelper, string actionName, string controllerName,
                                             object routeValues, string protocol)
        {
            string baseUrl = GetDomainBase(urlHelper, null, actionName, controllerName,
                                           new RouteValueDictionary(routeValues), protocol);
            return BuildUri(baseUrl, urlHelper.Action(actionName, controllerName, routeValues));
        }

        public static string SubdomainRouteUrl(this UrlHelper urlHelper, object routeValues)
        {
            return SubdomainRouteUrl(urlHelper, null, routeValues);
        }

        public static string SubdomainRouteUrl(this UrlHelper urlHelper, RouteValueDictionary routeValues)
        {
            return SubdomainRouteUrl(urlHelper, null, routeValues);
        }

        public static string SubdomainRouteUrl(this UrlHelper urlHelper, string routeName)
        {
            return SubdomainRouteUrl(urlHelper, routeName, (object)null);
        }

        public static string SubdomainRouteUrl(this UrlHelper urlHelper, string routeName, object routeValues)
        {
            return SubdomainRouteUrl(urlHelper, routeName, routeValues, null);
        }

        public static string SubdomainRouteUrl(this UrlHelper urlHelper, string routeName,
                                               RouteValueDictionary routeValues)
        {
            string baseUrl = GetDomainBase(urlHelper, routeName, null, null, routeValues);
            return BuildUri(baseUrl, urlHelper.RouteUrl(routeName, routeValues));
        }

        public static string SubdomainRouteUrl(this UrlHelper urlHelper, string routeName, object routeValues,
                                               string protocol)
        {
            var r = new RouteValueDictionary(routeValues);
            string baseUrl = GetDomainBase(urlHelper, routeName, null, null, r, protocol);
            return BuildUri(baseUrl, urlHelper.RouteUrl(routeName, routeValues));
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
        private static string GetDomainBase(UrlHelper urlHelper, string routeName, string actionName,
                                            string controllerName, RouteValueDictionary routeValues,
                                            string schema = null)
        {
            //baseUrl is the return value which by default is an empty string
            string baseUrl = string.Empty;

            //just a shortcut variable so we don't have to have this the below line eight million times
            Uri currentUrl = urlHelper.RequestContext.HttpContext.Request.Url;

            //get the desired route using a copy of MS internal methods
            RouteValueDictionary values = MergeRouteValues(actionName, controllerName, urlHelper.RequestContext.RouteData.Values, routeValues, true);
            VirtualPathData virtualPathForArea = urlHelper.RouteCollection.GetVirtualPathForArea(urlHelper.RequestContext, routeName, values);
            if (virtualPathForArea == null)
            {
                return baseUrl;
            }

            //if not a AttributeRoute or the current url is funny then nothing we can do so move on
            var route = virtualPathForArea.Route as IAttributeRoute;
            if (route != null && currentUrl != null && !string.IsNullOrWhiteSpace(currentUrl.OriginalString))
            {
                //get the current domain via the current Uri.
                string host = currentUrl.GetLeftPart(UriPartial.Authority).Replace(currentUrl.GetLeftPart(UriPartial.Scheme), string.Empty);

                IPAddress ip;
                //if the port exists in the host remove it so that we don't run into trouble with the IPAddress parsing
                if (host.Contains(":"))
                {
                    host = host.Substring(0, host.IndexOf(":", StringComparison.Ordinal));
                }

                //if an ip then no point in building a subdomain for it
                if (IPAddress.TryParse(host, out ip))
                {
                    return string.Empty;
                }

                //save the current host for comparisons later
                string currentHost = host;

                //which protocol schema to use. i.e. http, https
                string scheme = schema ?? currentUrl.Scheme;

                //what is the current port. needed if non-standard
                int port = currentUrl.Port;

                //is the port a standard port?
                bool useDefaultPort = port == 80 || port == 443;

                //need the default subdomain incase we are going from one subdomain method to a non-subdomain method
                string defaultSubdomain = string.Empty;
                if (route.DataTokens.Any(x => x.Key.Equals("defaultSubdomain")))
                {
                    defaultSubdomain = route.DataTokens["defaultSubdomain"].ToString();
                }

                //if the host contains a dot we need to remove the subdomain if it is in the list of ones to remove
                if (host.Contains("."))
                {
                    //get all registered subdomains
                    List<string> subdomains =
                        urlHelper.RouteCollection.Where(x => x is IAttributeRoute)
                                 .Cast<IAttributeRoute>()
                                 .Where(x => x.Subdomain.HasValue())
                                 .Select(x => x.Subdomain)
                                 .Distinct()
                                 .ToList();

                    //also add the default subdomain from the current route
                    if (!string.IsNullOrWhiteSpace(defaultSubdomain) && !subdomains.Contains(defaultSubdomain))
                    {
                        subdomains.Add(defaultSubdomain);
                    }

                    //strips subdomain information off of current (if matching a current one)
                    string subDomainSection = host.Split('.')[0];
                    foreach (
                        string subdomain in
                            subdomains.Where(
                                subdomain =>
                                subDomainSection.Equals(subdomain, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        host = host.Replace(string.Format("{0}.", subdomain), string.Empty);
                        break;
                    }
                }

                //if not a subdomain then don't build the url. instead build it to the default subdomain
                if (!string.IsNullOrWhiteSpace(route.Subdomain))
                {
                    //if host already starts with subdomain then skip building the url
                    if (!currentHost.StartsWith(route.Subdomain))
                    {
                        baseUrl = string.Format("{0}://{1}.{2}", scheme, route.Subdomain, host);
                    }
                }
                else
                {
                    //no subdomain so we should add the default subdomain unless it is localhost 
                    var gotoSubdomain = string.Empty;
                    if (!host.Equals("localhost", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(defaultSubdomain))
                    {
                        gotoSubdomain = string.Format("{0}.", defaultSubdomain);
                    }
                    baseUrl = string.Format("{0}://{1}{2}", scheme, gotoSubdomain, host);
                }

                //not using a standard port so if the baseurl has a value then append on the port
                if (!string.IsNullOrWhiteSpace(baseUrl) && !useDefaultPort)
                {
                    baseUrl = string.Format("{0}:{1}", baseUrl, port);
                }
            }
            return baseUrl;
        }

        private static string BuildUri(string baseUrl, string relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl) && string.IsNullOrWhiteSpace(relativeUrl))
            {
                return string.Empty;
            }
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return relativeUrl;
            }
            if (string.IsNullOrWhiteSpace(relativeUrl))
            {
                return baseUrl;
            }
            if (!relativeUrl.StartsWith("/"))
            {
                relativeUrl = "/" + relativeUrl;
            }
            return string.Format("{0}{1}", baseUrl, relativeUrl);
        }

        public static RouteValueDictionary GetRouteValues(RouteValueDictionary routeValues)
        {
            return routeValues == null ? new RouteValueDictionary() : new RouteValueDictionary(routeValues);
        }

        public static RouteValueDictionary MergeRouteValues(string actionName, string controllerName,
                                                            RouteValueDictionary implicitRouteValues,
                                                            RouteValueDictionary routeValues,
                                                            bool includeImplicitMvcValues)
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
                foreach (var keyValuePair in GetRouteValues(routeValues))
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