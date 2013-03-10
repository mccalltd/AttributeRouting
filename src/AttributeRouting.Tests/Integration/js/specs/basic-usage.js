/// <reference path="../lib/jasmine.js" />
/// <reference path="specs.js" />

describe('Basic Usage', function() {
    describe('BasicUsageController', function () {
        it('should respond to GET "/BasicUsage" with "BasicUsage.Index"', specs.magic);
        it('should respond to POST "/BasicUsage" with "BasicUsage.Create"', specs.magic);
        it('should respond to PUT "/BasicUsage/1" with "BasicUsage.Update(1)"', specs.magic);
        it('should respond to DELETE "/BasicUsage/1" with "BasicUsage.Delete(1)"', specs.magic);
    });
    describe('HttpBasicUsageController', function () {
        it('should respond to GET "/HttpBasicUsage" with "HttpBasicUsage.Index"', specs.magic);
        it('should respond to POST "/HttpBasicUsage" with "HttpBasicUsage.Create"', specs.magic);
        it('should respond to PUT "/HttpBasicUsage/1" with "HttpBasicUsage.Update(1)"', specs.magic);
        it('should respond to DELETE "/HttpBasicUsage/1" with "HttpBasicUsage.Delete(1)"', specs.magic);
    });
});
