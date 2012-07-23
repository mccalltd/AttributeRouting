using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Web.Areas.Api.Controllers
{
    [RouteVersioned(MinVer = "1.0")]
    public class VersionedController : BaseApiController
    {
        //
        // GET: /Versioned/
        [GET("Versioned", MinVer = "0.0")]
        public ActionResult Index()
        {
            return new ContentResult() { Content = "This is something" };
        }

        [GET("Versioned/{id}", MinVer="1.1")]
        public ActionResult Show(int id)
        {
            return new ContentResult() { Content = "This is something" };
        }

        [GET("Versioned/SingleVersion", MinVer="1.0", MaxVer="1.0")]
        public ActionResult New()
        {
            return new ContentResult() { Content = "This is something" };
        }

    }
}
