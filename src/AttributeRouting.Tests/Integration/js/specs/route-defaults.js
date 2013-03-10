/// <reference path="../lib/jasmine.js" />
/// <reference path="specs.js" />

describe('Route Defaults', function() {
    describe('RouteDefaultsController', function () {
        it('should respond to GET "/RouteDefaults/Inline" with "RouteDefaults.Inline(param, query)"', specs.magic);
        it('should respond to GET "/RouteDefaults/Optional" with "RouteDefaults.Optional(, )"', specs.magic);
        it('should respond to GET "/RouteDefaults/Optional/param" with "RouteDefaults.Optional(param, )"', specs.magic);
        it('should respond to GET "/RouteDefaults/Optional/?q=bleary" with "RouteDefaults.Optional(, bleary)"', specs.magic);
        it('should respond to GET "/RouteDefaults/ControllerName" with "RouteDefaults.ControllerName"', specs.magic);
        it('should respond to GET "/RouteDefaults/ActionName" with "RouteDefaults.ActionName"', specs.magic);
    });
    describe('HttpRouteDefaultsController', function () {
        it('should respond to GET "/HttpRouteDefaults/Inline" with "HttpRouteDefaults.Inline(param, query)"', specs.magic);
        it('should respond to GET "/HttpRouteDefaults/Optional" with "HttpRouteDefaults.Optional(, )"', specs.magic);
        it('should respond to GET "/HttpRouteDefaults/Optional/param" with "HttpRouteDefaults.Optional(param, )"', specs.magic);
        it('should respond to GET "/HttpRouteDefaults/Optional/?q=weary" with "HttpRouteDefaults.Optional(, weary)"', specs.magic);
        it('should respond to GET "/HttpRouteDefaults/ControllerName" with "HttpRouteDefaults.ControllerName"', specs.magic);
        it('should respond to GET "/HttpRouteDefaults/ActionName" with "HttpRouteDefaults.ActionName"', specs.magic);
    });
});
