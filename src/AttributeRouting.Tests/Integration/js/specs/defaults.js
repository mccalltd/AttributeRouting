/// <reference path="../lib/jasmine.js" />
/// <reference path="specs.js" />

describe('Route Defaults', function() {
    describe('DefaultsController', function () {
        it('should respond to GET "/Defaults/Inline" with "Defaults.Inline(param, query)"', specs.respondsWith);
        it('should respond to GET "/Defaults/Optional" with "Defaults.Optional(, )"', specs.respondsWith);
        it('should respond to GET "/Defaults/Optional/param" with "Defaults.Optional(param, )"', specs.respondsWith);
        it('should respond to GET "/Defaults/Optional/?q=bleary" with "Defaults.Optional(, bleary)"', specs.respondsWith);
        it('should respond to GET "/Defaults/ControllerName" with "Defaults.ControllerName"', specs.respondsWith);
        it('should respond to GET "/Defaults/ActionName" with "Defaults.ActionName"', specs.respondsWith);
    });
    describe('HttpDefaultsController', function () {
        it('should respond to GET "/HttpDefaults/Inline" with "HttpDefaults.Inline(param, query)"', specs.respondsWith);
        it('should respond to GET "/HttpDefaults/Optional" with "HttpDefaults.Optional(, )"', specs.respondsWith);
        it('should respond to GET "/HttpDefaults/Optional/param" with "HttpDefaults.Optional(param, )"', specs.respondsWith);
        it('should respond to GET "/HttpDefaults/Optional/?q=weary" with "HttpDefaults.Optional(, weary)"', specs.respondsWith);
        it('should respond to GET "/HttpDefaults/ControllerName" with "HttpDefaults.ControllerName"', specs.respondsWith);
        it('should respond to GET "/HttpDefaults/ActionName" with "HttpDefaults.ActionName"', specs.respondsWith);
    });
});
