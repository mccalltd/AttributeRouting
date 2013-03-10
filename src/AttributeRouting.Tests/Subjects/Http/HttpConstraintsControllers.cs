using System;
using System.Web.Http;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Subjects.Http
{
    [RoutePrefix("HttpConstraints")]
    public class HttpConstraintsController : ApiController
    {
        [GET("Alpha/{x:alpha}"), HttpGet]
        public string Alpha(string x)
        {
            return "HttpConstraints.Alpha({0})".FormatWith(x);
        }

        [GET("Int/{x:int}"), HttpGet]
        public string Int(int x)
        {
            return "HttpConstraints.Int({0})".FormatWith(x);
        }

        [GET("Long/{x:long}"), HttpGet]
        public string Long(long x)
        {
            return "HttpConstraints.Long({0})".FormatWith(x);
        }

        [GET("Float/{x:float}"), HttpGet]
        public string Float(float x)
        {
            return "HttpConstraints.Float({0})".FormatWith(x);
        }

        [GET("Double/{x:double}"), HttpGet]
        public string Double(double x)
        {
            return "HttpConstraints.Double({0})".FormatWith(x);
        }

        [GET("Decimal/{x:decimal}"), HttpGet]
        public string Decimal(decimal x)
        {
            return "HttpConstraints.Decimal({0})".FormatWith(x);
        }

        [GET("Bool/{x:bool}"), HttpGet]
        public string Bool(bool x)
        {
            return "HttpConstraints.Bool({0})".FormatWith(x);
        }

        [GET("Guid/{x:guid}"), HttpGet]
        public string Guid(Guid x)
        {
            return "HttpConstraints.Guid({0})".FormatWith(x);
        }

        [GET("DateTime/{x:datetime}"), HttpGet]
        public string DateTime(DateTime x)
        {
            return "HttpConstraints.DateTime({0})".FormatWith(x);
        }

        [GET("Length/{x:length(1)}"), HttpGet]
        public string Length(string x)
        {
            return "HttpConstraints.Length({0})".FormatWith(x);
        }

        [GET("MinLength/{x:minlength(5)}"), HttpGet]
        public string MinLength(string x)
        {
            return "HttpConstraints.MinLength({0})".FormatWith(x);
        }

        [GET("MaxLength/{x:maxlength(5)}"), HttpGet]
        public string MaxLength(string x)
        {
            return "HttpConstraints.MaxLength({0})".FormatWith(x);
        }

        [GET("LengthRange/{x:length(2, 3)}"), HttpGet]
        public string LengthRange(string x)
        {
            return "HttpConstraints.LengthRange({0})".FormatWith(x);
        }

        [GET("Min/{x:min(1)}"), HttpGet]
        public string Min(int x)
        {
            return "HttpConstraints.Min({0})".FormatWith(x);
        }

        [GET("Max/{x:max(10)}"), HttpGet]
        public string Max(int x)
        {
            return "HttpConstraints.Max({0})".FormatWith(x);
        }

        [GET("Range/{x:range(1, 10)}"), HttpGet]
        public string Range(int x)
        {
            return "HttpConstraints.Range({0})".FormatWith(x);
        }

        [GET(@"Regex/{x:regex(^howdy$)}"), HttpGet]
        public string Regex(string x)
        {
            return "HttpConstraints.Regex({0})".FormatWith(x);
        }

        [GET(@"RegexRange/{x:regex(\w{2,5})}"), HttpGet]
        public string RegexRange(string x)
        {
            return "HttpConstraints.RegexRange({0})".FormatWith(x);
        }

        [GET("EnumValue/{x:colorValue}"), HttpGet]
        public string EnumValue(Color x)
        {
            return "HttpConstraints.EnumValue({0})".FormatWith(x);
        }

        [GET("Enum/{x:color}"), HttpGet]
        public string Enum(Color x)
        {
            return "HttpConstraints.Enum({0})".FormatWith(x);
        }

        [GET("Compound/{x:int:max(10)}"), HttpGet]
        public string Compound(int x)
        {
            return "HttpConstraints.Compound({0})".FormatWith(x);
        }

        [GET("WithOptional/{x:color?}"), HttpGet]
        public string WithOptional(Color? x)
        {
            return "HttpConstraints.WithOptional({0})".FormatWith(x);
        }

        [GET("WithDefault/{x:color=red}"), HttpGet]
        public string WithDefault(Color x)
        {
            return "HttpConstraints.WithDefault({0})".FormatWith(x);
        }

        [GET("MultipleInSegment/{x:int}x{y:int}"), HttpGet]
        public string MultipleInSegment(int x, int y)
        {
            return "HttpConstraints.MultipleInSegment({0}, {1})".FormatWith(x, y);
        }

        [GET("Query?{x:int}&{y}"), HttpGet]
        public string Query(int x, string y)
        {
            return "HttpConstraints.Query({0}, {1})".FormatWith(x, y);
        }
    }

    [RoutePrefix("HttpConstraints/InRoutePrefix/{x:int}")]
    public class HttpConstraintsInRoutePrefixController : ApiController
    {
        [GET(""), HttpGet]
        public string Index(int x)
        {
            return "HttpConstraintsInRoutePrefix.Index({0})".FormatWith(x);
        }
    }
}