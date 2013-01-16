using System;
using System.Web.Http;
using AttributeRouting.Tests.Web.Models;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Web.Areas.Api.Controllers
{
    [RouteVersioned(MinVer = "1.0")]
    public class VersionedController : BaseApiController
    {
        //
        // GET: /Versioned/
        [HttpGet, GET("Versioned", MaxVer = "1.1")]
        public VersionedModel_old Index_old()
        {
            return new VersionedModel_old() {Text = "This is /versioned (up to 1.1)", GeneratedTime = DateTime.Now};
        }

        [HttpGet, GET("Versioned", MinVer = "1.2")]
        public VersionedModel Index()
        {
            return new VersionedModel()
                {
                    Title = "This is /versioned", 
                    Body = "This model is added in 1.2, and returns title/body isntead of just text", 
                    GeneratedTime = DateTime.Now
                };
        }

        [HttpGet, GET("Versioned/{id}", MinVer = "1.1")]
        public string Show(int id)
        {
            return string.Format("This is /versioned/id with id = {0}", id);
        }

        [HttpGet, GET("Versioned/SingleVersion", MinVer = "1.0", MaxVer = "1.0")]
        public string New()
        {
            return "This should only work with version 1.0";
        }

        [HttpGet, GET("Versioned/BeforeV1", MinVer = "0.0")]
        public string BeforeV1()
        {
            return "This existed in versions even prior to 1.0 (overrides class-level version)";
        }


    }
}
