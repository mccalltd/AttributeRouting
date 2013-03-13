using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Web.Http.Constraints;
using AttributeRouting.Web.Http.Framework;

namespace AttributeRouting.Web.Http
{
    public class HttpConfiguration : ConfigurationBase
    {
        public HttpConfiguration(bool inMemory = false)
        {
            AttributeRouteFactory = new AttributeRouteFactory(this);
            ParameterFactory = new RouteParameterFactory();
            RouteConstraintFactory = new RouteConstraintFactory(this);
            RegisterDefaultInlineRouteConstraints<IHttpRouteConstraint>(typeof(RegexHttpRouteConstraint).Assembly);

            if (inMemory)
            {
                AutoGenerateRouteNames = true;
                RouteNameBuilder = new UniqueRouteNameBuilder();
            }
        }

        /// <summary>
        /// The controller type applicable to this context.
        /// </summary>
        public override Type FrameworkControllerType
        {
            get { return typeof(IHttpController); }
        }

        /// <summary>
        /// The message handler that will be the recipient of the request.
        /// </summary>
        public HttpMessageHandler MessageHandler { get; set; }

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

        /// <summary>
        /// Appends the routes from the specified controller type to the end of route collection.
        /// </summary>
        /// <typeparam name="T">The controller type.</typeparam>
        public void AddRoutesFromController<T>() where T : IHttpController
        {
            AddRoutesFromController(typeof(T));
        }

        /// <summary>
        /// Appends the routes from all controllers that derive from the specified controller type to the route collection.
        /// </summary>
        /// <typeparam name="T">The base controller type.</typeparam>
        public void AddRoutesFromControllersOfType<T>() where T : IHttpController
        {
            AddRoutesFromControllersOfType(typeof(T));
        }
    }
}