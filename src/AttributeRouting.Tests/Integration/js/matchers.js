beforeEach(function() {
    this.addMatchers({
        toEqualValue: function (expected) {
            var actual = this.actual;
            this.message = function() {
                return "Expected " + actual + " to equal " + expected;
            };
            return actual.toLowerCase() === expected.toLowerCase();
        }
    });
});