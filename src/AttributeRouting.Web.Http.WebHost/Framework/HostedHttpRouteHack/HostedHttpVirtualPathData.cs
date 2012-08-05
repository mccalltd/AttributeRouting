// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Web.Http.Routing;
using System.Web.Routing;

namespace AttributeRouting.Web.Http.WebHost.Framework.HostedHttpRouteHack
{
    internal class HostedHttpVirtualPathData : IHttpVirtualPathData
    {
        private readonly VirtualPathData _virtualPath;

        public HostedHttpVirtualPathData(VirtualPathData virtualPath, IHttpRoute httpRoute)
        {
            if (virtualPath == null)
            {
                throw new ArgumentNullException("route");
            }

            _virtualPath = virtualPath;
            Route = httpRoute;
        }

        public IHttpRoute Route { get; private set; }

        public string VirtualPath
        {
            get { return _virtualPath.VirtualPath; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _virtualPath.VirtualPath = value;
            }
        }
    }
}
