using System.Web.Http;
using System.Web.Routing;
using AttributeRouting.Web.Http.WebHost;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.MapAttributeRoutes();
            GlobalConfiguration.Configuration.Routes.MapHttpAttributeRoutes();
        }
    }
}