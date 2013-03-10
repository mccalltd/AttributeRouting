/// <reference path="../lib/jasmine.js" />
/// <reference path="specs.js" />

describe('Route Constraints', function() {
    describe('ConstraintsController', function () {
        it('should respond to GET "/Constraints/Alpha/abc" with "Constraints.Alpha(abc)"', specs.respondsWith);
        it('should respond to GET "/Constraints/Alpha/123" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Int/123" with "Constraints.Int(123)"', specs.respondsWith);
        it('should respond to GET "/Constraints/Int/abc" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Long/123" with "Constraints.Long(123)"', specs.respondsWith);
        it('should respond to GET "/Constraints/Long/abc" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Float/256" with "Constraints.Float(256)"', specs.respondsWith);
        it('should respond to GET "/Constraints/Float/abc" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Double/256" with "Constraints.Double(256)"', specs.respondsWith);
        it('should respond to GET "/Constraints/Double/abc" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Decimal/256" with "Constraints.Decimal(256)"', specs.respondsWith);
        it('should respond to GET "/Constraints/Decimal/abc" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Bool/true" with "Constraints.Bool(True)"', specs.respondsWith);
        it('should respond to GET "/Constraints/Bool/false" with "Constraints.Bool(False)"', specs.respondsWith);
        it('should respond to GET "/Constraints/Bool/0" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Guid/C568BCD6-7D2B-4241-9191-F4B1ED4E1632" with "Constraints.Guid(C568BCD6-7D2B-4241-9191-F4B1ED4E1632)"', specs.respondsWith);
        it('should respond to GET "/Constraints/Guid/0" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/DateTime/2013-01-01" with "Constraints.DateTime(1/1/2013 12:00:00 am)"', specs.respondsWith);
        it('should respond to GET "/Constraints/DateTime/2013-9" with "Constraints.DateTime(9/1/2013 12:00:00 am)"', specs.respondsWith);
        it('should respond to GET "/Constraints/DateTime/January" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Length/a" with "Constraints.Length(a)"', specs.respondsWith);
        it('should respond to GET "/Constraints/Length/ab" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/MinLength/abcde" with "Constraints.MinLength(abcde)"', specs.respondsWith);
        it('should respond to GET "/Constraints/MinLength/abcd" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/MaxLength/abcde" with "Constraints.MaxLength(abcde)"', specs.respondsWith);
        it('should respond to GET "/Constraints/MaxLength/abcdef" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/LengthRange/ab" with "Constraints.LengthRange(ab)"', specs.respondsWith);
        it('should respond to GET "/Constraints/LengthRange/abc" with "Constraints.LengthRange(abc)"', specs.respondsWith);
        it('should respond to GET "/Constraints/LengthRange/a" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/LengthRange/abcd" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Min/1" with "Constraints.Min(1)"', specs.respondsWith);
        it('should respond to GET "/Constraints/Min/0" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Max/10" with "Constraints.Max(10)"', specs.respondsWith);
        it('should respond to GET "/Constraints/Max/11" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Range/1" with "Constraints.Range(1)"', specs.respondsWith);
        it('should respond to GET "/Constraints/Range/10" with "Constraints.Range(10)"', specs.respondsWith);
        it('should respond to GET "/Constraints/Range/0" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Range/11" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Regex/howdy" with Constraints.Regex(howdy)', specs.respondsWith);
        it('should respond to GET "/Constraints/Regex/doody" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/RegexRange/abcd" with Constraints.RegexRange(abcd)', specs.respondsWith);
        it('should respond to GET "/Constraints/RegexRange/a" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/EnumValue/0" with Constraints.EnumValue(Red)', specs.respondsWith);
        it('should respond to GET "/Constraints/EnumValue/Red" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Enum/Red" with Constraints.Enum(Red)', specs.respondsWith);
        it('should respond to GET "/Constraints/Enum/0" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Compound/5" with Constraints.Compound(5)', specs.respondsWith);
        it('should respond to GET "/Constraints/Compound/15" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Compound/abc" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/WithOptional/red" with Constraints.WithOptional(Red)', specs.respondsWith);
        it('should respond to GET "/Constraints/WithOptional" with Constraints.WithOptional()', specs.respondsWith);
        it('should respond to GET "/Constraints/WithDefault" with Constraints.WithDefault(Red)', specs.respondsWith);
        it('should respond to GET "/Constraints/WithDefault/green" with Constraints.WithDefault(Green)', specs.respondsWith);
        it('should respond to GET "/Constraints/MultipleInSegment/2x4" with Constraints.MultipleInSegment(2, 4)', specs.respondsWith);
        it('should respond to GET "/Constraints/MultipleInSegment/2x" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/MultipleInSegment/x4" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/MultipleInSegment/axb" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Query/?x=10&y=present" with Constraints.Query(10, present)', specs.respondsWith);
        it('should respond to GET "/Constraints/Query/?x=abc&y=present" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Query/?x=&y=present" with 404', specs.respondsWith);
        it('should respond to GET "/Constraints/Query/?x=10&y=" with 404', specs.respondsWith);
    });
    describe('ConstraintsInAreaPrefixController', function() {
        it('should respond to GET "/Constraints/InAreaPrefix/123" with ConstraintsInAreaPrefix.Index(123)', specs.respondsWith);
        it('should respond to GET "/Constraints/InAReaPrefix/abc" with 404', specs.respondsWith);
    });
    describe('ConstraintsInRoutePrefixController', function() {
        it('should respond to GET "/Constraints/InRoutePrefix/123" with ConstraintsInRoutePrefix.Index(123)', specs.respondsWith);
        it('should respond to GET "/Constraints/InRoutePrefix/abc" with 404', specs.respondsWith);
    });
});
