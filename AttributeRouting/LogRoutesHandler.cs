using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace AttributeRouting
{
    public class LogRoutesHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var writer = context.Response.Output;
            writer.NewLine = "<br/>";

            RouteTable.Routes.Cast<Route>().LogTo(writer);
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
