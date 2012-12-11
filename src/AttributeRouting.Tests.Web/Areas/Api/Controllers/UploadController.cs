using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Web.Areas.Api.Controllers
{
    /// <summary>
    /// To test issue 146
    /// </summary>
    [RoutePrefix("upload")]
    public class UploadController : BaseApiController
    {
        // POST /api/plain
        [POST("")]
        public string Post()
        {
            return "foo";
        }
    }
}