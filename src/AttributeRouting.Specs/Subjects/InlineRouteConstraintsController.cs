using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Specs.Subjects
{
    [RoutePrefix("Inline-Constraints")]
    public class InlineRouteConstraintsController : Controller
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

        [GET("Length/{x:length(1)}")]
        public string Length(string x)
        {
            return "";
        }

        [GET("MinLength/{x:minlength(10)}")]
        public string MinLength(string x)
        {
            return "";
        }

        [GET("MaxLength/{x:maxlength(10)}")]
        public string MaxLength(string x)
        {
            return "";
        }

        [GET("LengthRange/{x:length(1, 10)}")]
        public string LengthRange(string x)
        {
            return "";
        }

        [GET("Min/{x:min(1)}")]
        public string Min(int x)
        {
            return "";
        }

        [GET("Max/{x:max(10)}")]
        public string Max(int x)
        {
            return "";
        }

        [GET("Range/{x:range(1, 10)}")]
        public string Range(int x)
        {
            return "";
        }

        [GET(@"Regex/{x:regex(\d+)}")]
        public string Regex(int x)
        {
            return "";
        }

        [GET("Compound/{x:int:max(10)}")]
        public string Compound(int x)
        {
            return "";
        }
    }
}