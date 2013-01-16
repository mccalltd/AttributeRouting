using System.Web.Mvc;

namespace AttributeRouting.Tests.Web.Areas.Subdomain
{
    public class SubdomainAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Subdomain";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            /*context.MapRoute(
                "Subdomain_default",
                "Subdomain/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );*/
        }
    }
}
