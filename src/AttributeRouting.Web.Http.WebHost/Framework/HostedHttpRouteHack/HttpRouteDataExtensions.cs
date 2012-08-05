// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Web.Http.Routing;
using System.Web.Http.WebHost;
using System.Web.Routing;

namespace AttributeRouting.Web.Http.WebHost.Framework.HostedHttpRouteHack
{
    internal static class HttpRouteDataExtensions
    {
        public static RouteData ToRouteData(this IHttpRouteData httpRouteData)
        {
            if (httpRouteData == null)
            {
                throw new ArgumentNullException("httpRouteData");
            }

            HostedHttpRouteData hostedHttpRouteData = httpRouteData as HostedHttpRouteData;
            if (hostedHttpRouteData != null)
            {
                return hostedHttpRouteData.OriginalRouteData;
            }

            Route route = httpRouteData.Route.ToRoute();
            return new RouteData(route, HttpControllerRouteHandler.Instance);
        }
    }
}
