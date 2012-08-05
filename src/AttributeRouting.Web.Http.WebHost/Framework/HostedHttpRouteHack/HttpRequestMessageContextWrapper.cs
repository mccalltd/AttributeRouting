// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Net.Http;
using System.Web;

namespace AttributeRouting.Web.Http.WebHost.Framework.HostedHttpRouteHack
{
    internal class HttpRequestMessageContextWrapper : HttpContextBase
    {
        private HttpRequestMessageWrapper _httpWrapper;

        public HttpRequestMessageContextWrapper(string virtualPathRoot, HttpRequestMessage httpRequest)
        {
            _httpWrapper = new HttpRequestMessageWrapper(virtualPathRoot, httpRequest);
        }

        public override HttpRequestBase Request
        {
            get { return _httpWrapper; }
        }
    }
}
