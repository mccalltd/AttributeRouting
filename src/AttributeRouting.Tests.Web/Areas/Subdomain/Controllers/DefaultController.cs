using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Web.Areas.Subdomain.Controllers
{
    [RouteArea("Subdomain", Subdomain = "sub")]
    public class DefaultController : Controller
    {
        [GET("")]
        public string Index()
        {
            return "'tis the subdomain";
        }

    }
}
