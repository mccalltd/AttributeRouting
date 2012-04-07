using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Specs.Subjects.Http
{
    public class HttpRouteDefaultsController : ApiController
    {
        [GET("Default/{p1}")]
        [RouteDefault("p1", "variable")]
        public string Get()
        {
            return "";
        }

        [GET("InlineDefaults/{hello=sun}/{goodnight=moon}")]
        public string InlineDefaults()
        {
            return "";
        }

        [GET("Optionals/{p1?}/{?p2}/{?p3?}")]
        public string Optionals(string p1, string p2, string p3)
        {
            return "";
        }

        [GET("MultipleDefaults/1/{p1}", RouteName = "ApiMultipleDefaults1")]
        [GET("MultipleDefaults/2/{p1}", RouteName = "ApiMultipleDefaults2")]
        [RouteDefault("p1", "first", ForRouteNamed = "ApiMultipleDefaults1")]
        [RouteDefault("p1", "second", ForRouteNamed = "ApiMultipleDefaults2")]
        public string MultipleRoutes()
        {
            return "";
        }
    }
}