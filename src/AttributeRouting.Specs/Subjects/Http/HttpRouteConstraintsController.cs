using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Specs.Subjects.Http
{
    public class HttpRouteConstraintsController : ApiController
    {
        [GET("Constraint/{p1}")]
        [RegexRouteConstraint("p1", @"\d+")]
        public string Get()
        {
            return "";
        }

        [GET(@"InlineConstraints/{number(\d+)}/{word(\w{2})}/{alphanum([A-Za-z0-9]*)}/{capture((gotcha))}")]
        public string InlineConstraints(long number, string word)
        {
            return "";
        }

        [GET("MultipleConstraints/1/{p1}", RouteName = "ApiMultipleConstraints1")]
        [GET("MultipleConstraints/2/{p1}", RouteName = "ApiMultipleConstraints2")]
        [RegexRouteConstraint("p1", @"\d+", ForRouteNamed = "ApiMultipleConstraints1")]
        [RegexRouteConstraint("p1", @"\d{4}", ForRouteNamed = "ApiMultipleConstraints2")]
        public string MultipleRoutes()
        {
            return "";
        }
    }
}