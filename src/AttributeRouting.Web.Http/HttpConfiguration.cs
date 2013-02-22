using System.Web.Http.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Web.Http.Constraints;
using AttributeRouting.Web.Http.Framework;

namespace AttributeRouting.Web.Http
{
    public class HttpConfiguration : HttpConfigurationBase
    {
        public HttpConfiguration()
        {
            Init();
        }

        /// <summary>
        /// Automatically applies the specified constaint against url parameters
        /// with names that match the given regular expression.
        /// </summary>
        /// <param name="keyRegex">The regex used to match url parameter names</param>
        /// <param name="constraint">The constraint to apply to matched parameters</param>
        public void AddDefaultRouteConstraint(string keyRegex, IHttpRouteConstraint constraint)
        {
            base.AddDefaultRouteConstraint(keyRegex, constraint);
        }
    }
}
