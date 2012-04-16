﻿using System;
using System.Threading;
using System.Web;
using System.Web.Routing;
using AttributeRouting.Framework.Factories;
using AttributeRouting.Web.Framework;
using AttributeRouting.Web.Framework.Factories;

namespace AttributeRouting.Web {
    public abstract class WebAttributeRoutingConfiguration : AttributeRoutingConfigurationBase
    {
        private readonly IConstraintFactory _constraintFactory;

        protected WebAttributeRoutingConfiguration(Func<IRouteHandler> handlerFactory) {
            RouteHandlerFactory = handlerFactory;

            _constraintFactory = new ConstraintFactory();

            CurrentUICultureResolver = (ctx, data) => Thread.CurrentThread.CurrentUICulture.Name;
        }

        /// <summary>
        /// Constraint factory
        /// </summary>
        public override IConstraintFactory ConstraintFactory {
            get { return _constraintFactory; }
        }

        /// <summary>
        /// Automatically applies the specified constaint against url parameters
        /// with names that match the given regular expression.
        /// </summary>
        /// <param name="keyRegex">The regex used to match url parameter names</param>
        /// <param name="constraint">The constraint to apply to matched parameters</param>
        public void AddDefaultRouteConstraint(string keyRegex, IRouteConstraint constraint) {
            base.AddDefaultRouteConstraint(keyRegex, constraint);
        }

        public Func<IRouteHandler> RouteHandlerFactory { get; set; }

        /// <summary>
        /// Specifies a function that returns an alternate route handler.
        /// By default, the route handler is the default handler for the namespace type (Mvc, Http).
        /// </summary>
        /// <example>
        /// <code>
        /// routes.MapAttributeRoutes(config =>
        /// {
        ///    config.ScanAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        ///    config.UseRouteHandler(() => new MyOtherLibrary.Mvc.CustomRouteHandler());
        ///    // default:  config.UseRouteHandler(() => new System.Web.Mvc.MvcRouteHandler());
        /// });
        /// 
        /// routes.MapHttpAttributeRoutes(config =>
        /// {
        ///    config.ScanAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        ///    config.UseRouteHandler(() => new MyOtherLibrary.WebApi.CustomRouteHandler());
        ///    // default:  config.UseRouteHandler(() => System.Web.Http.WebHost.HttpControllerRouteHandler.Instance());
        /// });        
        /// </code>
        /// </example>
        /// <param name="routeHandlerFactory"></param>
        public void UseRouteHandler(Func<IRouteHandler> routeHandlerFactory) {
            RouteHandlerFactory = routeHandlerFactory;
        }

        /// <summary>
        /// this delegate returns the current UI culture name.
        /// This value is used when constraining inbound routes by culture <see cref="AttributeRoutingConfiguration{TRequestContext,TRouteData}.ConstrainTranslatedRoutesByCurrentUICulture"/>.
        /// The default delegate returns the CurrentUICulture name of the current thread.
        /// </summary>
        public Func<HttpContextBase, RouteData, string> CurrentUICultureResolver { get; set; }
    }
}
