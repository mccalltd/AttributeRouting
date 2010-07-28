using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AttributeRouting.Tests.Subjects.Controllers
{
    [RoutePrefix("Parent/{parentId}")]
    public class NestedTestController : Controller
    {
        [GET("Child")]
        public ActionResult Index(int parentId)
        {
            return Content("");
        }
    }
}
