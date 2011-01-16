using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RouteArea("Area")]
    [RoutePrefix("Parent/{parentId}")]
    public class AbsoluteUrlTestController : Controller
    {
        [GET("Something/Else", IsAbsoluteUrl = true)]
        public ActionResult Index()
        {
            return Content("");
        }
    }
}
