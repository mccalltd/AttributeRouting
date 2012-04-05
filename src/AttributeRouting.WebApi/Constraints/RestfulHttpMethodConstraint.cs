using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Common;
using System.Web.Http.Routing;

namespace AttributeRouting.WebApi
{
    /// <summary>
    /// Constrains a route by the specified allowed HTTP methods.
    /// </summary>
    public class RestfulHttpMethodConstraint : HttpMethodConstraint, IRestfulHttpMethodConstraint {
        /// <summary>
        /// Constrain a route by the specified allowed HTTP methods.
        /// </summary>
        public RestfulHttpMethodConstraint(params HttpMethod[] allowedMethods)
            : base(allowedMethods) {}

        ICollection<string> IRestfulHttpMethodConstraint.AllowedMethods
        {
            get { return new ReadOnlyCollection<string>(base.AllowedMethods.Select(method => method.Method).ToList()); }
        }
    }
}