using System.Collections.ObjectModel;
using System.Net.Http;

namespace AttributeRouting.WebApi {
    public interface IHttpRouteAttribute {
        Collection<HttpMethod> HttpMethods { get; } 
    }
}