using System.Web.Http.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Web.Http.Constraints;
using AttributeRouting.Web.Http.SelfHost.Framework.Factories;

namespace AttributeRouting.Web.Http.SelfHost
{
    public class HttpConfiguration : HttpConfigurationBase
    {
        public HttpConfiguration()
        {
            AttributeRouteFactory = new AttributeRouteFactory(this);
            RouteConstraintFactory = new RouteConstraintFactory(this);
            ParameterFactory = new RouteParameterFactory();

            RegisterDefaultInlineRouteConstraints<IHttpRouteConstraint>(typeof(RegexRouteConstraint).Assembly);

            // Must turn on AutoGenerateRouteNames and use the Unique RouteNameBuilder for this to work out-of-the-box.
            AutoGenerateRouteNames = true;
            RouteNameBuilder = RouteNameBuilders.Unique;
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
