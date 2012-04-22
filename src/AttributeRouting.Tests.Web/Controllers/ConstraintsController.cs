using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Web.Controllers
{
    [RoutePrefix("Constraints")]
    public class ConstraintsController : Controller
    {
        [GET("Int/{x:int}")]
        public string Int(int x)
        {
            return x.ToString();
        }

        [GET("IntOptional/{x:int?}")]
        public string IntOptional(int? x)
        {
            return x.GetValueOrDefault(-1).ToString();
        }

        [GET("IntDefault/{x:int=123}")]
        public string IntDefault(int x)
        {
            return x.ToString();
        }

        [GET("IntCompound/{x:int:max(10)}")]
        public string IntCompound(int x)
        {
            return x.ToString();
        }
    }
}
