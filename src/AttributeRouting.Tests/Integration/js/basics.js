/// <reference path="lib/jasmine.js" />
/// <reference path="specs.js" />

describe('Basic Usage', function() {
    describe('BasicsController', function () {
        it('responds to GET "/Basics" with "Basics.Index"', specs.respondsWith);
        it('responds to POST "/Basics" with "Basics.Create"', specs.respondsWith);
        it('responds to PUT "/Basics/1" with "Basics.Update(1)"', specs.respondsWith);
        it('responds to DELETE "/Basics/1" with "Basics.Delete(1)"', specs.respondsWith);
    });
    
    describe('HttpBasicsController', function () {
        it('responds to GET "/HttpBasics" with "HttpBasics.Index"', specs.respondsWith);
        it('responds to POST "/HttpBasics" with "HttpBasics.Create"', specs.respondsWith);
        it('responds to PUT "/HttpBasics/1" with "HttpBasics.Update(1)"', specs.respondsWith);
        it('responds to DELETE "/HttpBasics/1" with "HttpBasics.Delete(1)"', specs.respondsWith);
    });
});
