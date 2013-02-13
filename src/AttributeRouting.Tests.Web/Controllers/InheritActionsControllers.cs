using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Web.Controllers
{
    public abstract class InheritActionsControllerBase : Controller
    {
        [GET("Base-Method")]
        public string Index()
        {
            return "Base Index";
        }
    }

    [RoutePrefix("Inherit/Derived")]
    public class InheritActionsDerivedController : InheritActionsControllerBase
    {
        
    }
}
