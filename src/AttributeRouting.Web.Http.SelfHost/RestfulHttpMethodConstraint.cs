using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web.Http.SelfHost
{
    /// <summary>
    /// Constrains a route by the specified allowed HTTP methods.
    /// </summary>
    public class RestfulHttpMethodConstraint : HttpMethodConstraint, IRestfulHttpMethodConstraint
    {
        public RestfulHttpMethodConstraint(params HttpMethod[] allowedMethods)
            : base(allowedMethods) {}

        ICollection<string> IRestfulHttpMethodConstraint.AllowedMethods
        {
            get { return new ReadOnlyCollection<string>(AllowedMethods.Select(method => method.Method).ToList()); }
        }
    }
}