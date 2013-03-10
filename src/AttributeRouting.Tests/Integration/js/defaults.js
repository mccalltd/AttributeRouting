/// <reference path="lib/jasmine.js" />
/// <reference path="specs.js" />

describe('Route Defaults', function() {
    describe('DefaultsController', function () {
        it('responds to GET "/Defaults/Inline" with "Defaults.Inline(param, query)"', specs.respondsWith);
        it('responds to GET "/Defaults/Optional" with "Defaults.Optional(, )"', specs.respondsWith);
        it('responds to GET "/Defaults/Optional/param" with "Defaults.Optional(param, )"', specs.respondsWith);
        it('responds to GET "/Defaults/Optional/?q=bleary" with "Defaults.Optional(, bleary)"', specs.respondsWith);
        it('responds to GET "/Defaults/ControllerName" with "Defaults.ControllerName"', specs.respondsWith);
        it('responds to GET "/Defaults/ActionName" with "Defaults.ActionName"', specs.respondsWith);
    });
    describe('HttpDefaultsController', function () {
        it('responds to GET "/HttpDefaults/Inline" with "HttpDefaults.Inline(param, query)"', specs.respondsWith);
        it('responds to GET "/HttpDefaults/Optional" with "HttpDefaults.Optional(, )"', specs.respondsWith);
        it('responds to GET "/HttpDefaults/Optional/param" with "HttpDefaults.Optional(param, )"', specs.respondsWith);
        it('responds to GET "/HttpDefaults/Optional/?q=weary" with "HttpDefaults.Optional(, weary)"', specs.respondsWith);
        it('responds to GET "/HttpDefaults/ControllerName" with "HttpDefaults.ControllerName"', specs.respondsWith);
        it('responds to GET "/HttpDefaults/ActionName" with "HttpDefaults.ActionName"', specs.respondsWith);
    });
});
