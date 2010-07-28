using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AttributeRouting
{
    public class AttributeRoutingViewEngine : WebFormViewEngine
    {
        public AttributeRoutingViewEngine()
        {
            AreaMasterLocationFormats = new[] {
                "~/Views/{2}/{1}/{0}.master",
                "~/Views/{2}/Shared/{0}.master",
            };

			AreaViewLocationFormats = new[] {
                "~/Views/{2}/{1}/{0}.aspx",
                "~/Views/{2}/{1}/{0}.ascx",
                "~/Views/{2}/Shared/{0}.aspx",
                "~/Views/{2}/Shared/{0}.ascx",
            };

			AreaPartialViewLocationFormats = AreaViewLocationFormats;
        }
    }
}
