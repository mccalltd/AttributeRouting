using System;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Web.Http.Constraints;
using AttributeRouting.Web.Http.Framework;

namespace AttributeRouting.Web.Http
{
    public abstract class HttpConfigurationBase : ConfigurationBase
    {
        protected HttpConfigurationBase()
        {
            CurrentUICultureResolver = (ctx, data) => Thread.CurrentThread.CurrentUICulture.Name;
        }

        public void Init()
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
        /// Defines whether the Web API pipeline will run in memory or not.
        /// </summary>
        public bool InMemory { get; set; }

        /// <summary>
        /// The message handler that will be the recipient of the request.
        /// </summary>
        public HttpMessageHandler MessageHandler { get; set; }

        /// <summary>
        /// The controller type applicable to this context.
        /// </summary>
        public override Type FrameworkControllerType
        {
            get { return typeof(IHttpController); }
        }

        /// <summary>
        /// This delegate returns the current UI culture name,
        /// which is used when constraining inbound routes by culture.
        /// The default delegate returns the CurrentUICulture name of the current thread.
        /// </summary>
        public Func<HttpRequestMessage, IHttpRouteData, string> CurrentUICultureResolver { get; set; }

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