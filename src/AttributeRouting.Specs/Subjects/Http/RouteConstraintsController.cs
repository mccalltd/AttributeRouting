using System.Web.Http;
using AttributeRouting.Web;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Specs.Subjects.Http
{
    [RoutePrefix("Inline-Constraints")]
    public class ApiInlineRouteConstraintsController : ApiController
    {
        [GET("Int/{x:int}")]
        public string Int(int x)
        {
            return "";
        }
        
        [GET("Long/{x:long}")]
        public string Long(long x)
        {
            return "";
        }
        
        [GET("Float/{x:float}")]
        public string Float(float x)
        {
            return "";
        }
        
        [GET("Double/{x:double}")]
        public string Double(double x)
        {
            return "";
        }
        
        [GET("Decimal/{x:decimal}")]
        public string Decimal(decimal x)
        {
            return "";
        }

        [GET("Bool/{x:bool}")]
        public string Bool(bool x)
        {
            return "";
        }
    }

    public class ApiRouteConstraintsController : ApiController
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