using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;

namespace AttributeRouting.Web.Http.WebHost.Routing
{
    /// <summary>
    /// cf: Issue #90 - https://github.com/mccalltd/AttributeRouting/issues/90
    /// Workaround for a bug in System.Web.Http.Dispatcher.HttpRoutingDispatcher.
    /// Thanks HongMei Ge!
    /// </summary>
    public class RouteBypassingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var invoker = new HttpMessageInvoker(new HttpControllerDispatcher(request.GetConfiguration()));
            return invoker.SendAsync(request, cancellationToken);
        }
    }
}
