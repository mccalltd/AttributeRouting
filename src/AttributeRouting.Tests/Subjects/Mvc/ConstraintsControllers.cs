using System;
using System.Web.Mvc;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Mvc;

namespace AttributeRouting.Tests.Subjects.Mvc
{
    [RoutePrefix("Constraints")]
    public class ConstraintsController : Controller
    {
        [GET("Alpha/{x:alpha}")]
        public string Alpha(string x)
        {
            return "Constraints.Alpha({0})".FormatWith(x);
        }

        [GET("Int/{x:int}")]
        public string Int(int x)
        {
            return "Constraints.Int({0})".FormatWith(x);
        }

        [GET("Long/{x:long}")]
        public string Long(long x)
        {
            return "Constraints.Long({0})".FormatWith(x);
        }

        [GET("Float/{x:float}")]
        public string Float(float x)
        {
            return "Constraints.Float({0})".FormatWith(x);
        }

        [GET("Double/{x:double}")]
        public string Double(double x)
        {
            return "Constraints.Double({0})".FormatWith(x);
        }

        [GET("Decimal/{x:decimal}")]
        public string Decimal(decimal x)
        {
            return "Constraints.Decimal({0})".FormatWith(x);
        }

        [GET("Bool/{x:bool}")]
        public string Bool(bool x)
        {
            return "Constraints.Bool({0})".FormatWith(x);
        }

        [GET("Guid/{x:guid}")]
        public string Guid(Guid x)
        {
            return "Constraints.Guid({0})".FormatWith(x);
        }

        [GET("DateTime/{x:datetime}")]
        public string DateTime(DateTime x)
        {
            return "Constraints.DateTime({0})".FormatWith(x);
        }

        [GET("Length/{x:length(1)}")]
        public string Length(string x)
        {
            return "Constraints.Length({0})".FormatWith(x);
        }

        [GET("MinLength/{x:minlength(5)}")]
        public string MinLength(string x)
        {
            return "Constraints.MinLength({0})".FormatWith(x);
        }

        [GET("MaxLength/{x:maxlength(5)}")]
        public string MaxLength(string x)
        {
            return "Constraints.MaxLength({0})".FormatWith(x);
        }

        [GET("LengthRange/{x:length(2, 3)}")]
        public string LengthRange(string x)
        {
            return "Constraints.LengthRange({0})".FormatWith(x);
        }

        [GET("Min/{x:min(1)}")]
        public string Min(int x)
        {
            return "Constraints.Min({0})".FormatWith(x);
        }

        [GET("Max/{x:max(10)}")]
        public string Max(int x)
        {
            return "Constraints.Max({0})".FormatWith(x);
        }

        [GET("Range/{x:range(1, 10)}")]
        public string Range(int x)
        {
            return "Constraints.Range({0})".FormatWith(x);
        }

        [GET(@"Regex/{x:regex(^howdy$)}")]
        public string Regex(string x)
        {
            return "Constraints.Regex({0})".FormatWith(x);
        }

        [GET(@"RegexRange/{x:regex(\w{2,5})}")]
        public string RegexRange(string x)
        {
            return "Constraints.RegexRange({0})".FormatWith(x);
        }

        [GET("EnumValue/{x:colorValue}")]
        public string EnumValue(Color x)
        {
            return "Constraints.EnumValue({0})".FormatWith(x);
        }

        [GET("Enum/{x:color}")]
        public string Enum(Color x)
        {
            return "Constraints.Enum({0})".FormatWith(x);
        }

        [GET("Compound/{x:int:max(10)}")]
        public string Compound(int x)
        {
            return "Constraints.Compound({0})".FormatWith(x);
        }

        [GET("WithOptional/{x:color?}")]
        public string WithOptional(Color? x)
        {
            return "Constraints.WithOptional({0})".FormatWith(x);
        }

        [GET("WithDefault/{x:color=red}")]
        public string WithDefault(Color x)
        {
            return "Constraints.WithDefault({0})".FormatWith(x);
        }

        [GET("MultipleInSegment/{x:int}x{y:int}")]
        public string MultipleInSegment(int x, int y)
        {
            return "Constraints.MultipleInSegment({0}, {1})".FormatWith(x, y);
        }

        [GET("Query?{x:int}&{y}")]
        public string Query(int x, string y)
        {
            return "Constraints.Query({0}, {1})".FormatWith(x, y);
        }
    }

    [RouteArea("Constraints", AreaUrl = "Constraints/InAreaPrefix/{x:int}")]
    public class ConstraintsInAreaPrefixController : Controller
    {
        [GET("")]
        public string Index(int x)
        {
            return "ConstraintsInAreaPrefix.Index({0})".FormatWith(x);
        }
    }
    
    [RoutePrefix("Constraints/InRoutePrefix/{x:int}")]
    public class ConstraintsInRoutePrefixController : Controller
    {
        [GET("")]
        public string Index(int x)
        {
            return "ConstraintsInRoutePrefix.Index({0})".FormatWith(x);
        }
    }
}