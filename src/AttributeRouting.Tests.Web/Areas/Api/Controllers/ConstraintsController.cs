using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Web.Areas.Api.Controllers
{
    [RoutePrefix("Constraints")]
    public class ConstraintsController : BaseApiController
    {
        [GET("Int/{x:int}")]
        public string GetInt(int x)
        {
            return x.ToString();
        }

        [GET("IntOptional/{x:int?}")]
        public string GetIntOptional(int? x)
        {
            return x.GetValueOrDefault(-1).ToString();
        }

        [GET("IntDefault/{x:int=123}")]
        public string GetIntDefault(int x)
        {
            return x.ToString();
        }
    }
}
