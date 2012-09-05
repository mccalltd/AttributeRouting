using System;
using System.Threading;
using System.Web;
using System.Web.Routing;

namespace AttributeRouting.Web
{
    public abstract class WebAttributeRoutingConfigurationBase : AttributeRoutingConfigurationBase
    {
        protected WebAttributeRoutingConfigurationBase()
        {
            CurrentUICultureResolver = (ctx, data) => Thread.CurrentThread.CurrentUICulture.Name;

            RegisterDefaultInlineRouteConstraints<IRouteConstraint>(typeof(RegexRouteConstraintAttribute).Assembly);
        }

        /// <summary>
        /// Automatically applies the specified constraint against url parameters
        /// with names that match the given regular expression.
        /// </summary>
        /// <param name="keyRegex">The regex used to match url parameter names</param>
        /// <param name="constraint">The constraint to apply to matched parameters</param>
        public void AddDefaultRouteConstraint(string keyRegex, IRouteConstraint constraint)
        {
            base.AddDefaultRouteConstraint(keyRegex, constraint);
        }

        /// <summary>
        /// This delegate returns the current UI culture name,
        /// which is used when constraining inbound routes by culture.
        /// The default delegate returns the CurrentUICulture name of the current thread.
        /// </summary>
        public Func<HttpContextBase, RouteData, string> CurrentUICultureResolver { get; set; }
    }
}
