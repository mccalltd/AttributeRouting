using System.Web.Routing;
using AttributeRouting.Constraints;

namespace AttributeRouting.Web
{
    /// <summary>
    /// Constrains a route by the specified allowed HTTP methods.
    /// </summary>
    /// <remarks>
    /// System.Web.Routing and System.Web.Http.Routing both have their own HttpMethodConstraint class. 
    /// However, for logging and easier interoperability, we have our own.
    /// </remarks>
    public class RestfulHttpMethodConstraint : HttpMethodConstraint, IRestfulHttpMethodConstraint
    {
        /// <summary>
        /// Constrain a route by the specified allowed HTTP methods.
        /// </summary>
        public RestfulHttpMethodConstraint(params string[] allowedMethods)
            : base(allowedMethods) { }       
    }
}