using System.Web.Http;
using System.Web.Routing;
using AttributeRouting.Tests.Subjects;
using AttributeRouting.Web.Http.WebHost;
using AttributeRouting.Web.Mvc;
using WebConstraints = AttributeRouting.Web.Constraints;
using HttpConstraints = AttributeRouting.Web.Http.Constraints;

namespace AttributeRouting.Tests
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.MapAttributeRoutes(cfg =>
            {
                cfg.AddRoutesFromAssemblyOf<MvcApplication>();
                cfg.InlineRouteConstraints.Add("color", typeof(WebConstraints.EnumRouteConstraint<Color>));
                cfg.InlineRouteConstraints.Add("colorValue", typeof(WebConstraints.EnumValueRouteConstraint<Color>));
            });
            GlobalConfiguration.Configuration.Routes.MapHttpAttributeRoutes(cfg =>
            {
                cfg.AddRoutesFromAssemblyOf<MvcApplication>();
                cfg.InlineRouteConstraints.Add("color", typeof(HttpConstraints.EnumRouteConstraint<Color>));
                cfg.InlineRouteConstraints.Add("colorValue", typeof(HttpConstraints.EnumValueRouteConstraint<Color>));                
            });
        }
    }
}